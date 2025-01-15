# ✨ MentorLink ✨

**MentorLink** is a platform that connects mentors with students, supporting their personal and professional growth through targeted mentoring. The system automatically matches mentors with students based on their skills, goals, and preferences.

---

## 🌟 Key Features:

### 🔍 Mentor-Student Matching
- Intelligent algorithm that connects the right mentors with students.
- Matches based on skills, goals, and time preferences.

### 💬 Real-Time Communication
- Built-in chat for quick message exchange.
- Support for video conferencing during mentoring sessions.

### 📈 Progress Tracking
- Goal setting for mentoring sessions.
- Progress monitoring and regular summaries.

### 🌟 Ratings and Feedback
- Feature for evaluating mentoring sessions.
- Adding reviews and feedback after completed sessions.

---

## 🔧 Technologies:

### Frontend 🎨
- **React**: Dynamic library for building web applications.
- **Material-UI**: Modern UI components library.
- **React Query**: Data fetching and caching library.

### Backend 🛠️
- **ASP.NET Core**: Efficient and scalable API.
- **Entity Framework Core**: Database management for PostgreSQL.
- **SignalR**: Real-time communication.
- **ASP.NET Identity**: Security and authorization.

### Other Tools 🔬
- **PostgreSQL**: Relational database for storing user and session data.
- **Docker**: Containerization for easy deployment.
- **Twilio / WebRTC**: Real-time video conferencing.

---

## 🚀 How to Run the Project:

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/sniperdev/MentorLink.git
   ```

2. **Run the Backend**:
    - Navigate to the backend folder:
      ```bash
      cd backend
      ```
    - Install dependencies and start the application:
      ```bash
      dotnet run
      ```

3. **Run the Frontend**:
    - Navigate to the frontend folder:
      ```bash
      cd frontend
      ```
    - Install dependencies and start the application:
      ```bash
      npm install
      npm run start
      ```
	  
	  ### Setup for HTTPS
Before running the project, make sure to trust the ASP.NET Core developer certificate:

```bash
dotnet dev-certs https --trust

4. **Access the Application**:
   Open your browser and go to [http://localhost:4200](http://localhost:4200).