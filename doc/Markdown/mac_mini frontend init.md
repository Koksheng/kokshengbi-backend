After git clone

#### 1. if straight away run dev, will have error
![image](https://github.com/user-attachments/assets/44fd226a-d6ca-482b-8489-5f874d5608ad)

#### back up the yaml file, cause this cause to pnpm
![image](https://github.com/user-attachments/assets/4744e98a-2954-4a1d-884a-c451b9e840d6)

#### 2. cross-env: command not found
![image](https://github.com/user-attachments/assets/a0116f53-c498-4bb7-b113-86f21ea7eeac)


```
sudo npm install cross-env --save-dev
```
![image](https://github.com/user-attachments/assets/99c3b4d3-b01b-4d88-8a13-3c9cb3ff743e)


#### Permission Denied
![image](https://github.com/user-attachments/assets/4cf35f5a-da39-4c38-8280-efaf1a0951fe)

#### Fix Directory Permissions:
```
sudo chown -R $(whoami) /Users/guekoksheng/Code/self\ learn/Yuapi/yuapi-frontend
```
![image](https://github.com/user-attachments/assets/a9796af0-87ad-4f87-b0fa-e95364d06e0b)
