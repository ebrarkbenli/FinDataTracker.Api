import { useEffect, useMemo, useState } from 'react'
import './App.css'
import {
  addStock,
  deleteStock,
  getAllStocks,
  getTopStocks,
} from './api/stocksApi'

function App() {
  const [stocks, setStocks] = useState([])
  const [topStocks, setTopStocks] = useState([])
  const [symbol, setSymbol] = useState('')
  const [isLoading, setIsLoading] = useState(true)
  const [isAdding, setIsAdding] = useState(false)
  const [deletingId, setDeletingId] = useState(null)
  const [errorMessage, setErrorMessage] = useState('')

  const totalValue = useMemo(
    () => stocks.reduce((acc, item) => acc + item.price, 0),
    [stocks],
  )

  const highestStock = useMemo(() => {
    if (stocks.length === 0) return null
    return stocks.reduce((max, item) => (item.price > max.price ? item : max), stocks[0])
  }, [stocks])

  const refreshDashboard = async () => {
    setErrorMessage('')
    try {
      const [all, top] = await Promise.all([getAllStocks(), getTopStocks(5)])
      setStocks(all)
      setTopStocks(top)
    } catch (error) {
      setErrorMessage(error.message || 'Failed to load data.')
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    refreshDashboard()
  }, [])

  const onAddStock = async (event) => {
    event.preventDefault()
    const normalized = symbol.trim().toUpperCase()
    if (!normalized) return

    setIsAdding(true)
    setErrorMessage('')
    try {
      await addStock(normalized)
      setSymbol('')
      await refreshDashboard()
    } catch (error) {
      setErrorMessage(error.message || 'Could not add stock.')
    } finally {
      setIsAdding(false)
    }
  }

  const onDeleteStock = async (id) => {
    setDeletingId(id)
    setErrorMessage('')
    try {
      await deleteStock(id)
      await refreshDashboard()
    } catch (error) {
      setErrorMessage(error.message || 'Could not delete stock.')
    } finally {
      setDeletingId(null)
    }
  }

  return (
    <main className="dashboard">
      <header className="hero">
        <div>
          <p className="eyebrow">FinDataTracker</p>
          <h1>Stock Dashboard</h1>
          <p className="subtitle">Track your symbols with live prices from your API.</p>
        </div>
        <form className="add-form" onSubmit={onAddStock}>
          <label htmlFor="symbol">Add Symbol</label>
          <div className="input-row">
            <input
              id="symbol"
              type="text"
              value={symbol}
              onChange={(event) => setSymbol(event.target.value)}
              placeholder="AAPL"
              maxLength={10}
              autoComplete="off"
            />
            <button type="submit" disabled={isAdding}>
              {isAdding ? 'Adding...' : 'Add'}
            </button>
          </div>
        </form>
      </header>

      {errorMessage && <p className="error">{errorMessage}</p>}

      <section className="stats-grid">
        <article className="stat-card">
          <p>Total tracked</p>
          <h2>{stocks.length}</h2>
        </article>
        <article className="stat-card">
          <p>Portfolio total</p>
          <h2>${totalValue.toFixed(2)}</h2>
        </article>
        <article className="stat-card">
          <p>Highest stock</p>
          <h2>{highestStock ? `${highestStock.symbol} ($${highestStock.price.toFixed(2)})` : '-'}</h2>
        </article>
      </section>

      <section className="content-grid">
        <div className="panel">
          <div className="panel-head">
            <h3>All Stocks</h3>
          </div>
          {isLoading ? (
            <p className="muted">Loading stocks...</p>
          ) : stocks.length === 0 ? (
            <p className="muted">No stocks yet. Add your first symbol above.</p>
          ) : (
            <div className="table-wrap">
              <table>
                <thead>
                  <tr>
                    <th>Symbol</th>
                    <th>Price</th>
                    <th>Last Update</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  {stocks.map((stock) => (
                    <tr key={stock.id}>
                      <td>{stock.symbol}</td>
                      <td>${stock.price.toFixed(2)}</td>
                      <td>{new Date(stock.lastUpdated).toLocaleString()}</td>
                      <td>
                        <button
                          type="button"
                          className="delete-btn"
                          onClick={() => onDeleteStock(stock.id)}
                          disabled={deletingId === stock.id}
                        >
                          {deletingId === stock.id ? '...' : 'Delete'}
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
        <div className="panel">
          <div className="panel-head">
            <h3>Top 5 by Price</h3>
          </div>
          {isLoading ? (
            <p className="muted">Loading top list...</p>
          ) : topStocks.length === 0 ? (
            <p className="muted">No data available.</p>
          ) : (
            <ul className="top-list">
              {topStocks.map((stock, index) => (
                <li key={stock.id}>
                  <span>#{index + 1}</span>
                  <strong>{stock.symbol}</strong>
                  <em>${stock.price.toFixed(2)}</em>
                </li>
              ))}
            </ul>
          )}
        </div>
      </section>
    </main>
  )
}

export default App
