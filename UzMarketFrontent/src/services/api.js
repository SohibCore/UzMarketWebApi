// API Helper service for UzMarket
// Uses Vite dev server proxy in development, pointing to /api
const BASE_URL = import.meta.env.VITE_API_BASE_URL || '/api';
const FALLBACK_BASE_URLS = [
  BASE_URL,
  BASE_URL === '/api' ? 'http://localhost:5089/api' : null,
].filter(Boolean);

function normalizeRegisterPayload(userData, pinfl) {
  const safeName = userData?.fullName?.trim() || '';
  const safeShortName = userData?.shortName?.trim() || safeName.split(' ')[0] || 'User';
  const safeAddress = userData?.address?.trim() || 'Uzbekistan';
  const safePinfl = String(pinfl || userData?.pinfl || '').trim();

  return {
    dto: {
      userName: userData?.userName?.trim(),
      password: userData?.password,
      fullName: safeName,
      shortName: safeShortName,
      phoneNumber: userData?.phoneNumber?.trim(),
      address: safeAddress,
      dateOfBirth: userData?.dateOfBirth,
      passportSeries: userData?.passportSeries?.trim(),
      email: userData?.email?.trim(),
      pinfl: safePinfl,
    },
    pinfl: safePinfl,
  };
}

async function parseErrorMessage(response) {
  try {
    const text = await response.text();
    if (!text) return null;

    try {
      const parsed = JSON.parse(text);
      if (typeof parsed === 'string') return parsed;
      if (parsed?.message) return parsed.message;
      if (parsed?.title) return parsed.title;
      if (parsed?.detail) return parsed.detail;
      if (parsed?.error) return parsed.error;
      if (parsed?.errors) {
        if (Array.isArray(parsed.errors)) return parsed.errors.join(', ');
        if (typeof parsed.errors === 'string') return parsed.errors;
        if (typeof parsed.errors === 'object') {
          return Object.entries(parsed.errors)
            .map(([field, messages]) => {
              const text = Array.isArray(messages) ? messages.join(', ') : String(messages);
              return `${field}: ${text}`;
            })
            .join(' | ');
        }
      }
      return text;
    } catch {
      return text;
    }
  } catch {
    return null;
  }
}

/**
 * Helper to perform fetch calls with credentials and JSON headers.
 */
async function request(endpoint, options = {}) {
  const normalizedEndpoint = endpoint.startsWith('/') ? endpoint : `/${endpoint}`;
  const timeoutMs = options.timeout ?? 8000;
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), timeoutMs);
  const method = (options.method || 'GET').toString().toUpperCase();
  const shouldRetryFallback = options.retry !== false && (method === 'GET' || method === 'HEAD');

  let lastError = null;

  for (let index = 0; index < FALLBACK_BASE_URLS.length; index += 1) {
    const baseUrl = FALLBACK_BASE_URLS[index];
    const url = `${baseUrl}${normalizedEndpoint}`;

    const requestOptions = {
      ...options,
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Content-Type': 'application/json',
        ...(options.headers || {}),
      },
    };

    if (requestOptions.body && typeof requestOptions.body === 'object') {
      requestOptions.body = JSON.stringify(requestOptions.body);
    }

    try {
      const response = await fetch(url, requestOptions);

      if (response.status === 401) {
        const err = new Error('Unauthorized');
        err.status = 401;
        throw err;
      }

      if (!response.ok) {
        const status = response.status;
        if (shouldRetryFallback && (status === 404 || status === 405) && index < FALLBACK_BASE_URLS.length - 1) {
          continue;
        }

        const errorMessage = await parseErrorMessage(response) || `API Error: ${status}`;
        const err = new Error(errorMessage);
        err.status = status;
        throw err;
      }

      const contentType = response.headers.get('content-type');
      if (contentType && contentType.includes('application/json')) {
        return await response.json();
      }

      const text = await response.text();
      if (text === 'true') return true;
      if (text === 'false') return false;
      if (text && !isNaN(Number(text))) return Number(text);
      return text;
    } catch (error) {
      if (error.name === 'AbortError') {
        const timeoutError = new Error('Request timed out');
        timeoutError.status = 504;
        throw timeoutError;
      }

      if (error?.status) {
        throw error;
      }

      lastError = error;
      if (shouldRetryFallback && index < FALLBACK_BASE_URLS.length - 1) {
        continue;
      }

      console.error(`Request to ${url} failed:`, error);
      const enhancedError = new Error(`Failed to fetch ${url}`);
      enhancedError.status = error?.status || 0;
      enhancedError.cause = error;
      enhancedError.url = url;
      throw enhancedError;
    }
  }

  throw lastError || new Error('API request failed');
}

export const api = {
  // --- Auth Endpoints ---
  auth: {
    login: async (userName, password) => {
      return request('/Auth/Login', {
        method: 'POST',
        body: { userName, password },
      });
    },
    register: async (userData, pinfl) => {
      return request('/Auth/Register', {
        method: 'POST',
        retry: false,
        timeout: 30000,
        body: normalizeRegisterPayload(userData, pinfl),
      });
    },
    verifyEmail: async (email, code) => {
      return request('/Auth/VerifyEmail/verify-email', {
        method: 'POST',
        retry: false,
        body: { email, code },
      });
    },
    me: async () => {
      return request('/Auth/Me', { method: 'GET' });
    },
    logout: async () => {
      return request('/Auth/Logout', { method: 'POST' });
    },
  },

  // --- Uzasbo Endpoints ---
  uzasbo: {
    getPersonInfo: async (pinfl) => {
      return request(`/Uzasbo/Get/${pinfl}`, { method: 'GET' });
    },
  },

  // --- Product Endpoints ---
  products: {
    getList: async (filters = {}) => {
      const queryParams = new URLSearchParams();
      if (filters.name) queryParams.append('Name', filters.name);
      if (filters.description) queryParams.append('Description', filters.description);
      if (filters.price) queryParams.append('Price', filters.price);
      
      const queryString = queryParams.toString();
      const endpoint = `/Product/GetList${queryString ? `?${queryString}` : ''}`;
      return request(endpoint, { method: 'GET' });
    },
    get: async (id) => {
      return request(`/Product/Get/${id}`, { method: 'GET' });
    },
    create: async (productData) => {
      const normalizedPrice = Number(productData.price);
      const normalizedStock = Number(productData.stockQuantity);
      const normalizedCategoryId = Number(productData.categoryId || 0);
      const imageItems = (productData.items || productData.tables || productData.images || []).map((item, index) => ({
        imageUrl: item.imageUrl || item.url || item,
        mainPic: Boolean(item.mainPic ?? (index === 0)),
        sortOrder: item.sortOrder ?? (index + 1),
      }));

      return request('/Product/Create', {
        method: 'POST',
        body: {
          name: productData.name,
          description: productData.description || '',
          price: Number.isFinite(normalizedPrice) ? normalizedPrice : 0,
          stockQuantity: Number.isFinite(normalizedStock) ? normalizedStock : 0,
          categoryId: Number.isFinite(normalizedCategoryId) ? normalizedCategoryId : 0,
          items: imageItems,
        },
      });
    },
    update: async (productData) => {
      const normalizedPrice = Number(productData.price);
      const normalizedStock = Number(productData.stockQuantity);
      const normalizedCategoryId = Number(productData.categoryId || 0);
      const imageItems = (productData.items || productData.tables || productData.images || []).map((item, index) => ({
        id: item.id || 0,
        imageUrl: item.imageUrl || item.url || item,
        mainPic: Boolean(item.mainPic ?? (index === 0)),
        sortOrder: item.sortOrder ?? (index + 1),
        productId: productData.id,
      }));

      return request('/Product/Update', {
        method: 'PATCH',
        body: {
          id: productData.id,
          name: productData.name,
          description: productData.description || '',
          price: Number.isFinite(normalizedPrice) ? normalizedPrice : 0,
          stockQuantity: Number.isFinite(normalizedStock) ? normalizedStock : 0,
          categoryId: Number.isFinite(normalizedCategoryId) ? normalizedCategoryId : 0,
          supplierId: productData.supplierId || 0,
          items: imageItems,
        },
      });
    },
    delete: async (id) => {
      return request(`/Product/Delete/${id}`, { method: 'DELETE' });
    },
  },

  // --- Cart Endpoints ---
  cart: {
    getList: async () => {
      return request('/Cart/GetList', { method: 'GET' });
    },
    get: async (id) => {
      return request(`/Cart/Get/${id}`, { method: 'GET' });
    },
    create: async (cartData) => {
      const itemsList = cartData.items || cartData.tables || [];
      const normalizedItems = itemsList.map(item => ({
        productId: item.productId ?? item.ProductId ?? item.id ?? item.Id ?? 0,
        quantity: Number(item.quantity ?? 1),
      }));

      return request('/Cart/Create', {
        method: 'POST',
        body: {
          items: normalizedItems,
        },
      });
    },
    update: async (cartData) => {
      const itemsList = cartData.tables || cartData.items || [];
      const normalizedItems = itemsList.map(item => ({
        id: item.id ?? item.Id ?? 0,
        productId: item.productId ?? item.ProductId,
        quantity: Number(item.quantity ?? 1),
      }));

      return request('/Cart/Update', {
        method: 'PATCH',
        retry: false,
        body: {
          id: cartData.id,
          statusId: cartData.statusId,
          items: normalizedItems,
        },
      });
    },
    delete: async (id) => {
      return request(`/Cart/Delete/${id}`, { method: 'DELETE' });
    },
  },

  // --- Order Endpoints ---
  orders: {
    getList: async (filters = {}) => {
      const queryParams = new URLSearchParams();
      const queryString = queryParams.toString();
      const endpoint = `/Order/GetList${queryString ? `?${queryString}` : ''}`;
      try {
        return await request(endpoint, { method: 'GET' });
      } catch (error) {
        if (error?.status === 401 || error?.status === 404 || error?.status === 405 || error?.status === 0) {
          return [];
        }
        throw error;
      }
    },
    get: async (id) => {
      return request(`/Order/Get/${id}`, { method: 'GET' });
    },
    create: async (orderData) => {
      // CreateOrderDlDto format: { orderDate: "YYYY-MM-DD", shippingAddressId, items: [ { productId, quantity, price } ] }
      const itemsList = orderData.items || orderData.tables || [];
      return request('/Order/Create', {
        method: 'POST',
        body: {
          orderDate: orderData.orderDate,
          shippingAddressId: orderData.shippingAddressId,
          items: itemsList.map(item => ({
            productId: item.productId,
            quantity: item.quantity,
            price: item.price
          }))
        },
      });
    },
    update: async (orderData) => {
      return request('/Order/Update', {
        method: 'PATCH',
        body: orderData,
      });
    },
    delete: async (id) => {
      return request(`/Order/Delete/${id}`, { method: 'DELETE' });
    },
  },

  // --- Category Endpoints ---
  categories: {
    getList: async () => {
      return request('/Category/GetList', { method: 'GET' });
    },
    get: async (id) => {
      return request(`/Category/Get/${id}`, { method: 'GET' });
    },
    create: async (categoryData) => {
      return request('/Category/Create', {
        method: 'POST',
        body: categoryData,
      });
    },
    update: async (categoryData) => {
      return request('/Category/Update', {
        method: 'PATCH',
        body: categoryData,
      });
    },
    delete: async (id) => {
      return request(`/Category/Delete/${id}`, { method: 'DELETE' });
    },
  },

  // --- Address Endpoints ---
  addresses: {
    getList: async () => {
      return request('/Address/GetList', { method: 'GET' });
    },
    get: async (id) => {
      return request(`/Address/Get/${id}`, { method: 'GET' });
    },
    create: async (addressData) => {
      return request('/Address/Create', {
        method: 'POST',
        body: addressData,
      });
    },
    update: async (addressData) => {
      return request('/Address/Update', {
        method: 'PATCH',
        body: addressData,
      });
    },
    delete: async (id) => {
      return request(`/Address/Delete/${id}`, { method: 'DELETE' });
    },
  },

  // --- User Endpoints ---
  users: {
    getList: async (filters = {}) => {
      return request('/User/GetList', { method: 'GET' });
    },
    get: async (id) => {
      return request(`/User/Get/${id}`, { method: 'GET' });
    },
    create: async (userData) => {
      return request('/User/Create', {
        method: 'POST',
        body: userData,
      });
    },
    update: async (userData) => {
      return request('/User/Update', {
        method: 'PATCH',
        body: userData,
      });
    },
    delete: async (id) => {
      return request(`/User/Delete/${id}`, { method: 'DELETE' });
    },
  }
};
