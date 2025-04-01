# 🛒 ModuCart - Modular E-Commerce API (in development)

ModuCart is a **scalable, modular, and secure** e-commerce platform built using **ASP.NET Core Microservices**. It leverages **Ocelot API Gateway**, **JWT authentication**, and **Consul service discovery** for seamless integration between services.

## 🚀 Features

✅ **Microservices Architecture** – Decoupled services for scalability  
✅ **Secure Payments API** – Integrated with industry-standard payment gateways  
✅ **Identity & Authentication** – Secure JWT-based authentication with Identity Server  
✅ **API Gateway with Ocelot** – Unified entry point for all services  
✅ **Consul Service Discovery** – Dynamic service registration & load balancing  
✅ **Resilient & Fault-Tolerant** – Implemented with Polly for retries & circuit breaking  

## 🏗️ Architecture

📦 ModuCart.ApiGateway - API Gateway using Ocelot  
📦 ModuCart.Identity - Authentication & User Management  
📦 ModuCart.Products - Product Management Microservice  
📦 ModuCart.Orders - Order Processing Microservice  
📦 ModuCart.Payments - Secure Payment Microservice  
📦 ModuCart.SharedKernel - Shared libraries and utilities


## 🛠️ Tech Stack

- **C# / .NET 9**
- **ASP.NET Core Web API**
- **Ocelot API Gateway**
- **Consul Service Discovery**
- **Polly for Resilience**
- **JWT Authentication**
- **Swagger for API Documentation**
- **Mongo database**
- **Docker & Kubernetes (Optional for Deployment)**

🔒 Authentication & Security

- Uses JWT Tokens for secure access
- Role-based access control (RBAC) for different user roles
- HTTPS enforced for all services

📜 API Documentation

Each microservice provides its own Swagger UI.
To access the API Gateway Swagger, visit:

```http://localhost:5000/swagger/index.html```

🏗️ Deployment

You can deploy using Docker or Kubernetes:

- Using Docker:
```docker-compose up --build -d```

- Using Kubernetes:
```kubectl apply -f k8s/```

🤝 Contributing

1. Fork the repository
2. Create a new branch (git checkout -b feature/your-feature)
3. Commit your changes (git commit -m 'feat: add new feature')
4. Push to the branch (git push origin feature/your-feature)
5. Open a Pull Request

📜 License

This project is licensed under the MIT License.

💡 Built with love & microservices! 🚀
