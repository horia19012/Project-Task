# Project Setup

## Requirements
- Docker
- Docker Compose

## Setup

After cloning the repository, create a `.env` file in the root folder, it must have this content with the variables:

MSSQL_SA_PASSWORD=YourUserPassword123!
GROQ_API_KEY=your_groq_api_key

##Run the project
docker compose up --build

##Stop the project
docker compose down

##Notes
The project uses Groq API because it is cheap, accessible, and provides efficient token usage for LLM requests.
Make sure Docker is running before starting the project.
