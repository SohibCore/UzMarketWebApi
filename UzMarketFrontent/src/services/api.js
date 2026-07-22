// API Helper service for UzMarket
// Uses Vite dev server proxy in development, pointing to /api
const BASE_URL = '/api';

/**
 * Helper to perform fetch calls with credentials and JSON headers.
 */
async function request(endpoint, options = {}) {
  const url = `${BASE_URL}${endpoint}`;
  const timeoutMs = options.timeout ?? 8000;
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), timeoutMs);
  
  // Ensure cookies are sent (needed for Cookie-based Auth)
  options.credentials = 'include';
  options.signal = controller.signal;
  
  options.headers = {
    'Content-Type': 'application/json',
    ...options.headers,
  };

  if (options.body && typeof options.body === 'object') {
    options.body = JSON.stringify(options.body);
  }

  try {
    const response = await fetch(url, options);
    
    if (response.status === 401) {
      // Return null or throw unauthorized
      const err = new Error('Unauthorized');
      err.status = 401;
      throw err;
    }
    
    if (!response.ok) {
      let errorMessage = `API Error: ${response.status}`;
      try {
        const text = await response.text();
        errorMessage = text || errorMessage;
      } catch (e) {
        // ignore
      }
      const err = new Error(errorMessage);
      err.status = response.status;
      throw err;
    }

    // Some endpoints return empty body on Success (e.g. Logout, Delete)
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      return await response.json();
    }
    
    return await response.text();
  } catch (error) {
    if (error.name === 'AbortError') {
      const timeoutError = new Error('Request timed out');
      timeoutError.status = 504;
      throw timeoutError;
    }

    console.error(`Request to ${url} failed:`, error);
    throw error;
  } finally {
    clearTimeout(timeoutId);
  }
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
        body: productData,
      });
    },
    update: async (productData) => {
      return request('/Product/Update', {
        method: 'PATCH',
        body: productData,
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
      // cartData format: { tables: [ { productId, quantity } ] }
      return request('/Cart/Create', {
        method: 'POST',
        body: cartData,
      });
    },
    update: async (cartData) => {
      // cartData format: { id, tables: [ { cartId, productId, quantity } ] }
      return request('/Cart/Update', {
        method: 'PATCH',
        body: cartData,
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
      // filters: can pass any parameters supported by backend
      const queryString = queryParams.toString();
      const endpoint = `/Order/GetList${queryString ? `?${queryString}` : ''}`;
      return request(endpoint, { method: 'GET' });
    },
    get: async (id) => {
      return request(`/Order/Get/${id}`, { method: 'GET' });
    },
    create: async (orderData) => {
      // orderData format: { orderDate: "YYYY-MM-DD", shippingAddressId, tables: [ { productId, quantity, price } ] }
      return request('/Order/Create', {
        method: 'POST',
        body: orderData,
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
