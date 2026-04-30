const BASE_URL = "http://localhost:5229/api";

function getHeaders() {
    return {
        "Content-Type": "application/json"
    };
}

async function handleResponse(res) {
    if (!res.ok) {
        const body = await res.json().catch(() => null);
        throw new Error(body?.message || res.statusText || 'API Error');
    }
    return res.json();
}

async function fetchCourses() {
    const res = await fetch(`${BASE_URL}/courses`, { headers: getHeaders() });
    return handleResponse(res);
}

async function fetchLessons(courseId) {
    const res = await fetch(`${BASE_URL}/courses/${courseId}/lessons`, { headers: getHeaders() });
    return handleResponse(res);
}

async function fetchDashboard(userId) {
    const res = await fetch(`${BASE_URL}/users/${userId}/dashboard`, { headers: getHeaders() });
    return handleResponse(res);
}

async function fetchProfile(userId) {
    const res = await fetch(`${BASE_URL}/users/${userId}/profile`, { headers: getHeaders() });
    return handleResponse(res);
}

async function fetchUser(userId) {
    const res = await fetch(`${BASE_URL}/users/${userId}`, { headers: getHeaders() });
    return handleResponse(res);
}

async function fetchResults(userId) {
    const res = await fetch(`${BASE_URL}/results/${userId}`, { headers: getHeaders() });
    return handleResponse(res);
}

async function fetchQuestions(quizId) {
    const res = await fetch(`${BASE_URL}/quizzes/${quizId}/questions`, { headers: getHeaders() });
    return handleResponse(res);
}

async function completeCourseApi(userId, courseId) {
    const res = await fetch(`${BASE_URL}/courses/complete`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify({ userId, courseId })
    });
    return handleResponse(res);
}

async function submitQuizApi(quizId, data) {
    const res = await fetch(`${BASE_URL}/quizzes/${quizId}/submit`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify(data)
    });
    return handleResponse(res);
}
