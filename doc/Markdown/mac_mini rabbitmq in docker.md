### Install RabbitMQ in Docker

https://www.rabbitmq.com/docs/download


- this will remove the container when u stop **(not recommeded)**
```
# latest RabbitMQ 3.13 
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
```


- this will not remove the container when u stop **(recommeded this)**
```
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
```

Example running the Command **(2nd time run with Recommended way)**
![image](https://github.com/user-attachments/assets/2bdcbcf2-9d1c-43cb-acdd-8d5fd7437799)
![image](https://github.com/user-attachments/assets/43b0f4af-2729-447c-af8f-8d5d6bcaf690)


Tricky way when installing RabbitMQ

```
docker pull rabbitmq:3.13-management
```

**Error**: `Error response from daemon: Get https://registry-1.docker.io/v2/: net/http: TLS handshake timeout`

**Solution**: 
```
docker pull rabbitmq:3.13-management || docker pull rabbitmq:3.13-management || docker pull rabbitmq:3.13-management || docker pull rabbitmq:3.13-management
```
Helping answer
https://serverfault.com/questions/908141/docker-pull-tls-handshake-timeout


Now aldy successfully installed

**Docker Images:**
![image](https://github.com/user-attachments/assets/dc238d7d-49ab-46d9-b9ba-399bea3b3b1a)

**Docker Containers:** just click the start button on Actions

![image](https://github.com/user-attachments/assets/b284d237-080f-4fa5-87f7-5789cc1da5fa)


**Access to RabbitMQ** http://localhost:15672/#/

![image](https://github.com/user-attachments/assets/aee1eb39-378e-4dfa-8d2e-77657f9af959)
