# JokesWorld
RestAPI WCF Windows service with jokes and WPF client's application. Users can create account, add jokes, add comments, edit their data.

---

### To run server as windows service:	

1. Publish your server app with: ```dotnet publish --configuration Release```

2. After publish remember to copy your database file from project directory to publish directory.

3. Create service: ```sc create [ServiceName] binPath= "path\To\your\publish\folder\Server.exe"``` ( space after binPath is important)

4. Start your service ```sc start [ServiceName]```

5. To stop your service use: ```sc stop [ServiceName]```

6. To delete your service use: ```sc delete [ServiceName]```

   #### There is a bit different URL

   You have to change ServerURL for correct one in client's application AppSettings.

   * for DEBUG mode: ```url: https://localhost:44377/jokeserver/api/users```

   * for Windows Service mode: ```url:http//localhost:5000/api/users```

---



### Login Screen:

<img src="imgs/login.png" alt="LoginScreen" style="zoom:80%;" />

---



### Register Screen:

<img src="imgs/register.png" alt="Register Screen" style="zoom:80%;" />

---



### Home Screen:

<img src="imgs/home.png" alt="Home Screen" style="zoom:50%;" />

---



### Users Screen:

<img src="imgs/users.png" alt="Users Screen" style="zoom:50%;" />

---



### Jokes Screen:

<img src="imgs/jokes.png" alt="Jokes Screen" style="zoom:50%;" />

---



### Comments Screen:

<img src="imgs/comments.png" alt="Comments Screen" style="zoom:50%;" />

---



### Settings Screen:

<img src="imgs/settings.png" alt="Settings Screen" style="zoom:50%;" />

---



### Single User Screen:

<img src="imgs/single_user.png" alt="Single User Screen" style="zoom:50%;" />

---



### Single Joke Screen:

<img src="imgs/single_joke.png" alt="Single Joke Screen" style="zoom:50%;" />

---



### Single Comment Screen:

<img src="imgs/single_comment.png" alt="Single Comment Screen" style="zoom:50%;" />