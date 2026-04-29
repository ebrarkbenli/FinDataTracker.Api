const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || ''

async function request(path, options = {}) {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(options.headers || {}),
    },
    ...options,
  })

  if (!response.ok) {
    let message = 'Request failed.'
    try {
      message = await response.text()
    } catch {
      message = 'Request failed.'
    }
    throw new Error(message)
  }

  if (response.status === 204) {
    return null
  }

  return response.json()
}

export const getAllStocks = () => request('/api/stocks')

export const getTopStocks = (count = 5) => request(`/api/stocks/top?count=${count}`)

export const getAvaragePrice = () => request('/api/stocks/avarage-price')

export const addStock = (symbol) =>
  request(`/api/stocks/${encodeURIComponent(symbol)}`, { method: 'POST' })

export const deleteStock = (id) =>
  request(`/api/stocks/${id}`, { method: 'DELETE' })
