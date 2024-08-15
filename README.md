## Smart BI Analytics

.NET Core + React fullstack project

**Frontend**: https://github.com/Koksheng/kokshengbi-frontend

**Backend**: https://github.com/Koksheng/kokshengbi-backend

## Project Description

An intelligent data analysis platform based on .NET Core + MQ + AIGC + React. 

Unlike traditional BI, users only need to upload raw data and input their analysis goal to automatically generate analysis conclusions and visual charts, thereby reducing costs and increasing efficiency in data analysis.

## Tech Stack

### Frontend
- **Framework**: React, Umi
- **Scaffolding**: Ant Design Pro
- **Component library**: Ant Design, Ant Design Components
- **Plugin**: OpenAPI frontend code generation

### Backend
- **Framework**: .NET Core 
- **Database**: Microsoft SQL Server
- **OpenAI API**: Model gpt-3.5-turbo (AIGC)
- **Rate Limiting**: Redis (limits the same user to 2 requests per second for the same function)
- **Message Queue**: RabbitMQ 

## Login
![image](https://github.com/user-attachments/assets/dcafe35b-f0ea-4df0-a1b4-728d49ccf1e2)

## Register
![image](https://github.com/user-attachments/assets/589c90d9-56ff-438b-b9eb-7ebc55fa6ce4)

## Intelligent Analysis 
![image](https://github.com/user-attachments/assets/4bc151ed-a3fb-4a67-91c9-be1bfa9049de)

## Intelligent Analysis (Async): 

  1. Submit Request
      ![image](https://github.com/user-attachments/assets/4d9c63bc-03a0-4300-abb8-ea281b69b6be)

  2. Processing Task
     ![image](https://github.com/user-attachments/assets/1e8b60f6-055d-4e09-8ae5-b2b7a6039e70)

  3. Reload to View Response 
     ![image](https://github.com/user-attachments/assets/a9bf05c2-2c6d-4fbc-8cb9-99700d4aca1f)


## My Chart
![image](https://github.com/user-attachments/assets/944e9950-7b63-4fef-bbd0-13ae0de64d7f)


## Project Structure

### Basic Structure
- **User**: Inputs analysis goals and uploads raw data
- **Backend (.NET Core)**: Validates, compresses, and processes the data
- **AI Service**: Generates charts and analysis conclusions
- **Backend (.NET Core)**: Further processes and manages user data
- **Database**: Stores the processed data

![image](https://github.com/user-attachments/assets/53dbdc3b-0030-4283-819f-72d76ca409d3)


### Asynchronous Structure
- **User**: Inputs analysis goals and uploads raw data
- **Backend (.NET Core)**: Validates, compresses, and processes the data; enforces rate limiting of 2 requests per second; sends the data to the message queue
- **Rate Limiting**: Limits each user to 2 requests per second
- **Message Queue**: Acts as an intermediary between the message producer and receiver
- **Task Processing Module**: Consumes and processes messages using the AI Service
- **AI Service**: Generates charts and analysis conclusions
- **Backend (.NET Core)**: Further processes the data and updates the analysis status
- **Database**: Stores the processed data

![image](https://github.com/user-attachments/assets/512b50c9-974f-4447-8e17-673ca4530ae8)




### kokshengbi-backend.sln
**Solution File**: This is the main solution file that includes all the backend projects.

### kokshengbi.Api (Start)
**Function**: This project likely serves as the main API application.

**Features**:
Exposes different endpoints for clients.
Handles incoming HTTP requests and processes them.
Includes controllers, such as the UserController, ChartController.

### kokshengbi.Application
**Function**: This project contains the application logic and services.

**Features**:
Implements business rules.
Provides application services that interact with the domain and infrastructure layers.
Contains interfaces and implementations for various operations needed by the **kokshengbi.Api**.

### kokshengbi.Contracts
**Function**: This project defines the data transfer objects (DTOs) and contracts used for communication.

**Features**:
Defines request and response models for API endpoints.
Ensures consistency in the data exchanged between the client and server.

### kokshengbi.Domain
**Function**: This project contains the domain models and business logic.

**Features**:
Defines the core entities and value objects.
Implements domain services and business rules.
Contains domain events and other domain-related logic.

### kokshengbi.Infrastructure
**Function**: This project handles the infrastructure concerns like data access and external service integrations.

**Features**:
Implements repositories and data context for database interactions.
Contains configurations and setups for external services.
Handles data persistence and retrieval operations.

**Services**:
- **OpenAiService**: Manages interactions with the OpenAI API.
- **RedisRateLimiterService**: Implements rate limiting for API requests.
- **BiMessageProducer**: Sends messages to RabbitMQ.
- **BiMessageConsumer**: Consumes messages from RabbitMQ.


