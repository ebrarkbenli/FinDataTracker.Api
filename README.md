# Financial Data Tracker

This is a full-stack application built with .NET Web API and React to track stock prices.
The backend fetches stock data from the Finnhub API, saves it to a local SQLite database, and exposes REST endpoints.
The frontend (React) provides a simple UI to interact with these endpoints.
# Technologies

## Backend

.NET Web API
Entity Framework Core
SQLite
Finnhub API
Swagger

## Frontend

React
Axios
Basic CSS

# Why Finnhub?

I chose Finnhub because it provides real-time stock data and has a generous free tier (60 requests per minute).
Alpha Vantage was another option but it has a stricter limit (25 requests per day).
SQLite is used because it is easy to run locally and does not require any setup.

# Main Entity
Stock:
Id
Symbol
Price
LastUpdated
Design
Repository pattern is used to keep database logic separate
Clean separation between backend and frontend

# Endpoints
GET /api/stocks
GET /api/stocks/{symbol}
POST /api/stocks/{symbol}
DELETE /api/stocks/{id}
GET /api/stocks/top?count=5
GET /api/stocks/average-price
# Project Structure
FinDataTracker.Api/
│
├── backend/      → .NET Web API
├── frontend/     → React app
# Setup
1. Clone the repository
git clone YOUR_REPOSITORY_URL
cd FinDataTracker.Api

2. Backend Setup
cd backend
dotnet user-secrets init
dotnet user-secrets set "Finnhub:ApiKey" "YOUR_API_KEY"
dotnet run
Backend will run on:
http://localhost:5033

# Swagger:
http://localhost:5033/swagger

3. Frontend Setup
cd ../frontend
npm install
npm start
Frontend will run on:
http://localhost:3000

## Notes
API key is not stored in the repository
Each user must provide their own Finnhub API key
User secrets are used for secure local development
