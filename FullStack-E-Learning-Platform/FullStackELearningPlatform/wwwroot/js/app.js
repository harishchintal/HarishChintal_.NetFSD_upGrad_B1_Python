document.addEventListener("DOMContentLoaded", () => {
    if (window.location.pathname.endsWith('Dashboard.html') ||
        window.location.pathname.endsWith('Courses.html') ||
        window.location.pathname.endsWith('Quiz.html') ||
        window.location.pathname.endsWith('Profile.html')) {
        const userId = localStorage.getItem('userId');
        if (!userId) {
            window.location.href = 'index.html';
            return;
        }
        setNavUser();
    }

    highlightActiveNav();

    if (document.getElementById('totalCourses')) {
        const userId = localStorage.getItem('userId');
        loadDashboard(userId);
    }

    if (document.getElementById('courseContainer')) {
        loadCourses();
    }

    if (document.getElementById('profileTotalCourses')) {
        const userId = localStorage.getItem('userId');
        loadProfile(userId);
    }
});

function setNavUser() {
    const userName = localStorage.getItem('userName') || 'Student';
    const userElement = document.getElementById('navUser');
    if (userElement) {
        userElement.textContent = `Hi, ${userName}`;
    }
}

async function loadDashboard(userId) {
    try {
        const data = await fetchDashboard(userId);
        const userName = localStorage.getItem('userName');
        const welcomeTitle = document.getElementById('welcomeTitle');
        if (welcomeTitle && userName) {
            welcomeTitle.innerText = `Welcome back, ${userName}`;
        }
        document.getElementById('totalCourses').innerText = data.totalCourses ?? 0;
        document.getElementById('completedCount').innerText = data.completedCourses ?? 0;
        document.getElementById('progressBar').value = data.progress ?? 0;
        document.getElementById('progressText').innerText = `${data.progress ?? 0}% Completed`;
    } catch (error) {
        console.error('Dashboard error:', error);
    }
}

async function loadCourses() {
    try {
        const courses = await fetchCourses();
        const container = document.getElementById('courseContainer');
        const tableBody = document.getElementById('courseTable');
        container.innerHTML = '';
        tableBody.innerHTML = '';
        if (!courses || courses.length === 0) {
            container.innerHTML = '<p>No courses available</p>';
            return;
        }
        courses.forEach(course => {
            const lessonCount = course.lessons?.length ?? 0;
            const totalDuration = course.lessons?.reduce((sum, l) => sum + (l.durationHours || 0), 0) ?? 0;
            const quizCount = course.quizzes?.length ?? 0;
            const firstQuizId = course.quizzes?.[0]?.quizId;
            const quizButton = firstQuizId
                ? `<button onclick="startQuiz(${firstQuizId})">Start Quiz</button>`
                : `<button disabled>No Quiz Available</button>`;
            const lessonsMarkup = (course.lessons || []).map(lesson => `<li>${lesson.title} (${lesson.durationHours}h)</li>`).join('');
            const card = document.createElement('div');
            card.className = 'card course-card';
            card.innerHTML = `
                <h3>${course.title}</h3>
                <p>${course.description}</p>
                <p><strong>Lessons:</strong> ${lessonCount}</p>
                <p><strong>Quizzes:</strong> ${quizCount}</p>
                <p><strong>Duration:</strong> ${totalDuration} hrs</p>
                <ul>${lessonsMarkup}</ul>
                <button onclick="markComplete(${course.courseId})">Mark Complete</button>
                ${quizButton}
            `;
            container.appendChild(card);
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${course.title}</td>
                <td>${lessonCount}</td>
                <td>${totalDuration} hrs</td>
                <td>${quizCount}</td>
            `;
            tableBody.appendChild(row);
        });
    } catch (error) {
        console.error('Courses error:', error);
    }
}

function startQuiz(quizId) {
    if (!quizId) {
        alert('No quiz available for this course.');
        return;
    }
    localStorage.setItem('quizId', quizId);
    window.location.href = 'Quiz.html';
}

async function markComplete(courseId) {
    try {
        const userId = localStorage.getItem('userId');
        if (!userId) {
            window.location.href = 'index.html';
            return;
        }
        await completeCourseApi(userId, courseId);
        alert('✅ Course marked complete. Progress updated.');
        const currentPage = window.location.pathname;
        if (currentPage.endsWith('Dashboard.html')) {
            loadDashboard(userId);
        }
        if (currentPage.endsWith('Courses.html')) {
            loadCourses();
        }
    } catch (error) {
        console.error('Complete error:', error);
        alert('❌ Failed to complete course: ' + error.message);
    }
}

async function loadProfile(userId) {
    try {
        const data = await fetchProfile(userId);
        document.getElementById('profileName').innerText = data.fullName || localStorage.getItem('userName');
        document.getElementById('profileEmail').innerText = data.email || localStorage.getItem('userEmail');
        document.getElementById('profileTotalCourses').innerText = data.totalCourses ?? 0;
        document.getElementById('profileCompletedCourses').innerText = data.completedCourses ?? 0;
        const progress = data.totalCourses > 0 ? Math.round((data.completedCourses / data.totalCourses) * 100) : 0;
        document.getElementById('profileProgress').innerText = `${progress}%`;
        document.getElementById('profileQuizResult').innerHTML = data.lastScore > 0
            ? `<strong>Score:</strong> ${data.lastScore} | <strong>Grade:</strong> ${data.lastGrade} | <strong>Feedback:</strong> ${data.lastFeedback}`
            : 'No quiz results yet.';
        const completedList = document.getElementById('completedCoursesList');
        completedList.innerHTML = '';
        if (data.completedCoursesNames && data.completedCoursesNames.length > 0) {
            data.completedCoursesNames.forEach(name => {
                const li = document.createElement('li');
                li.textContent = name;
                completedList.appendChild(li);
            });
        } else {
            completedList.innerHTML = '<li>No courses completed yet</li>';
        }
        const results = await fetchResults(userId);
        const resultTable = document.getElementById('resultTable');
        resultTable.innerHTML = '';
        if (results && results.length > 0) {
            results.forEach(item => {
                const row = document.createElement('tr');
                row.className = 'result-row';
                row.innerHTML = `
                    <td>${item.courseName}</td>
                    <td>${item.score}</td>
                    <td>${item.grade}</td>
                    <td>${item.feedback}</td>
                    <td>${new Date(item.attemptDate).toLocaleDateString()}</td>
                `;
                resultTable.appendChild(row);
            });
        } else {
            resultTable.innerHTML = '<tr><td colspan="5">No quiz results yet</td></tr>';
        }
    } catch (error) {
        console.error('Profile error:', error);
    }
}

function logout() {
    localStorage.clear();
    window.location.href = 'index.html';
}

function highlightActiveNav() {
    const links = document.querySelectorAll('nav a');
    links.forEach(link => {
        if (window.location.href.includes(link.getAttribute('href'))) {
            link.classList.add('active');
        }
    });
}
