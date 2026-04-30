const BASE_URL = "http://localhost:5229/api";

// COMMON HEADERS
function getHeaders() {
    return {
        "Content-Type": "application/json",
        "Authorization": "Bearer " + localStorage.getItem("token")
    };
}

// ================= ADD COURSE =================
async function addCourse() {

    const title = document.getElementById("courseTitle").value;
    const description = document.getElementById("courseDesc").value;

    const res = await fetch(`${BASE_URL}/courses`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify({ title, description })
    });

    if (!res.ok) return alert("Error adding course");

    alert("✅ Course Added");
}

// ================= ADD LESSON =================
async function addLesson() {

    const courseId = parseInt(document.getElementById("lessonCourseId").value);
    const title = document.getElementById("lessonTitle").value;
    const content = document.getElementById("lessonContent").value;
    const durationHours = parseInt(document.getElementById("lessonDuration").value);

    const res = await fetch(`${BASE_URL}/lessons`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify({ courseId, title, content, durationHours })
    });

    if (!res.ok) return alert("Error adding lesson");

    alert("✅ Lesson Added");
}

// ================= ADD QUIZ =================
async function addQuiz() {

    const courseId = parseInt(document.getElementById("quizCourseId").value);
    const title = document.getElementById("quizTitle").value;

    const res = await fetch(`${BASE_URL}/quizzes`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify({ courseId, title })
    });

    if (!res.ok) return alert("Error adding quiz");

    alert("✅ Quiz Added");
}

// ================= ADD QUESTION =================
async function addQuestion() {

    const quizId = parseInt(document.getElementById("qQuizId").value);

    const data = {
        quizId,
        questionText: document.getElementById("qText").value,
        optionA: document.getElementById("qA").value,
        optionB: document.getElementById("qB").value,
        optionC: document.getElementById("qC").value,
        optionD: document.getElementById("qD").value,
        correctAnswer: document.getElementById("qAns").value
    };

    const res = await fetch(`${BASE_URL}/questions`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify(data)
    });

    if (!res.ok) return alert("Error adding question");

    alert("✅ Question Added");
}