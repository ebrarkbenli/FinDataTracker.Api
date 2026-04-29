# Financial Data Tracker

This is a simple .NET Web API project to track stock prices.

The app fetches stock data from Finnhub API, saves it to a local SQLite database, and provides basic endpoints to manage stocks.

## Technologies

- .NET Web API
- Entity Framework Core
- SQLite
- Finnhub API
- Swagger

## Why Finnhub?

I chose Finnhub because it provides real-time stock data and has a generous free tier (60 requests per minute).

Alpha Vantage was another option but it has a stricter limit (25 requests per day).

SQLite is used because it is easy to run locally and does not require any setup.

## Main Entity

Stock:
- Id
- Symbol
- Price
- LastUpdated

## Design

// Repository pattern used to keep database logic separate

## Endpoints

GET /api/stocks  
GET /api/stocks/{symbol}  
POST /api/stocks/{symbol}  
DELETE /api/stocks/{id}  
GET /api/stocks/top?count=5  
GET /api/stocks/average-price  

## Setup

```bash
git clone YOUR_REPOSITORY_URL
cd FinDataTracker.Api

Notes
This project is intentionally kept simple.
It focuses on fetching stock data, saving them locally, and exposing basic API endpoints.