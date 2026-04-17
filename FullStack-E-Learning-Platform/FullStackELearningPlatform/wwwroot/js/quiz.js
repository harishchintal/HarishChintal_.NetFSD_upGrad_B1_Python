let timeLeft = 45;
let timerInterval;

async function displayQuiz() {
    const quizId = Number(localStorage.getItem('quizId'));
    const quizArea = document.getElementById('quizArea');
    const quizTitle = document.getElementById('quizTitle');
    quizArea.innerHTML = '';
    if (!quizId || quizId <= 0) {
        quizTitle.innerText = 'No quiz selected';
        quizArea.innerHTML = '<p>Please select a quiz from the Courses page.</p>';
        document.getElementById('submitQuizButton').disabled = true;
        return;
    }
    quizTitle.innerText = `Quiz #${quizId}`;
    try {
        const questions = await fetchQuestions(quizId);
        if (!questions || questions.length === 0) {
            quizArea.innerHTML = '<p>No questions are available for this quiz.</p>';
            document.getElementById('submitQuizButton').disabled = true;
            return;
        }
        questions.forEach((q, i) => {
            quizArea.innerHTML += `
                <div>
                    <p><strong>Q${i + 1}. ${q.questionText}</strong></p>
                    <label><input type="radio" name="q${i}" value="A"> ${q.optionA}</label><br>
                    <label><input type="radio" name="q${i}" value="B"> ${q.optionB}</label><br>
                    <label><input type="radio" name="q${i}" value="C"> ${q.optionC}</label><br>
                    <label><input type="radio" name="q${i}" value="D"> ${q.optionD}</label><br>
                </div>
            `;
        });
        timeLeft = 45;
        document.getElementById('submitQuizButton').disabled = false;
        startTimer();
    } catch (error) {
        quizArea.innerHTML = '<p>Unable to load quiz questions.</p>';
        console.error('Quiz load failed', error);
    }
}

function startTimer() {
    const timer = document.getElementById('timer');
    timerInterval = setInterval(() => {
        if (timeLeft <= 0) {
            clearInterval(timerInterval);
            timer.innerText = '⏱ Time Left: 0s';
            submitQuiz();
            return;
        }
        timer.innerText = `⏱ Time Left: ${timeLeft}s`;
        timeLeft -= 1;
    }, 1000);
}

async function submitQuiz() {
    clearInterval(timerInterval);
    const answerNodes = document.querySelectorAll('#quizArea div');
    const answers = [];
    for (let i = 0; i < answerNodes.length; i++) {
        const selected = document.querySelector(`input[name='q${i}']:checked`);
        if (!selected) {
            alert('Please answer all questions before submitting.');
            return;
        }
        answers.push(selected.value);
    }
    const quizId = Number(localStorage.getItem('quizId'));
    const userId = Number(localStorage.getItem('userId'));
    if (!userId || !quizId) {
        alert('No user or quiz selected. Please login and choose a quiz.');
        window.location.href = 'index.html';
        return;
    }
    try {
        const result = await submitQuizApi(quizId, { userId, answers });
        const resultArea = document.getElementById('result');
        resultArea.innerHTML = `
            <h3>Score: ${result.percentage}%</h3>
            <p><strong>Grade:</strong> ${result.grade}</p>
            <p><strong>Feedback:</strong> ${result.feedback}</p>
        `;
    } catch (error) {
        alert('Failed to submit quiz: ' + error.message);
        console.error(error);
    }
}

document.addEventListener('DOMContentLoaded', displayQuiz);
