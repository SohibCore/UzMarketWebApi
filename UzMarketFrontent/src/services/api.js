// API Helper service for UzMarket
// Uses Vite dev server proxy in development, pointing to /api
const BASE_URL = import.meta.env.VITE_API_BASE_URL || '/api';
const FALLBACK_BASE_URLS = [
  BASE_URL,
  BASE_URL === '/api' ? 'http://localhost:5089/api' : null,
].filter(Boolean);

/**
 * Helper to perform fetch calls with credentials and JSON headers.
 */
async function request(endpoint, options = {}) {
  const normalizedEndpoint = endpoint.startsWith('/') ? endpoint : `/${endpoint}`;
  const timeoutMs = options.timeout ?? 8000;
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), timeoutMs);

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
        if ((status === 404 || status === 405) && index < FALLBACK_BASE_URLS.length - 1) {
          continue;
        }

        let errorMessage = `API Error: ${status}`;
        try {
          const text = await response.text();
          errorMessage = text || errorMessage;
        } catch (e) {
          // ignore
        }
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

      lastError = error;
      if (index < FALLBACK_BASE_URLS.length - 1) {
        continue;
      }

      console.error(`Request to ${url} failed:`, error);
      throw error;
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
    register: async (userData) => {
      return request('/Auth/Register', {
        method: 'POST',
        body: userData,
      });
    },
    me: async () => {
      return request('/Auth/Me', { method: 'GET' });
    },
    logout: async () => {
      return request('/Auth/Logout', { method: 'POST' });
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
      return request('/Product/Create', {
        method: 'POST',
        body: {
          name: productData.name,
          description: productData.description || '',
          price: productData.price,
          stockQuantity: productData.stockQuantity,
          categoryId: productData.categoryId,
          tables: productData.tables || productData.images || []
        },
      });
    },
    update: async (productData) => {
      return request('/Product/Update', {
        method: 'PATCH',
        body: {
          id: productData.id,
          name: productData.name,
          description: productData.description || '',
          price: productData.price,
          stockQuantity: productData.stockQuantity,
          categoryId: productData.categoryId,
          tables: productData.tables || productData.images || []
        },
      });
    },
    delete: async (id) => {
      return request(`/Product/Delete/${id}`, { method: 'DELETE' });
    },
  },

  // --- Cart Endpoints ---
  cart: {
    getList: async (filters = {}) => {
      const queryParams = new URLSearchParams();
      if (filters.id) queryParams.append('Id', filters.id);
      
      const queryString = queryParams.toString();
      const endpoint = `/Cart/GetList${queryString ? `?${queryString}` : ''}`;
      return request(endpoint, { method: 'GET' });
    },
    get: async (id) => {
      return request(`/Cart/Get/${id}`, { method: 'GET' });
    },
    create: async (cartData) => {
      // CreateCartDlDto format: { userId, items: [ { productId, quantity } ] }
      const itemsList = cartData.items || cartData.tables || [];
      return request('/Cart/Create', {
        method: 'POST',
        body: {
          userId: cartData.userId,
          items: itemsList.map(item => ({
            productId: item.productId,
            quantity: item.quantity
          }))
        },
      });
    },
    update: async (cartData) => {
      // UpdateCartDlDto format: { id, statusId, tables: [ { id, productId, quantity } ] }
      const itemsList = cartData.tables || cartData.items || [];
      return request('/Cart/Update', {
        method: 'PATCH',
        body: {
          id: cartData.id,
          statusId: cartData.statusId || 2, // 2 = MODIFIED
          tables: itemsList.map(item => ({
            id: item.id || null,
            productId: item.productId,
            quantity: item.quantity
          }))
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
      return request(endpoint, { method: 'GET' });
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
