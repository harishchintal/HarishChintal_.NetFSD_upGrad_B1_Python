function loadQuiz() {
    return new Promise(function (resolve) {
        setTimeout(function () {
            resolve(questions);
        }, 1000);
    });
}

async function displayQuiz() {

    var quizArea = document.getElementById("quizArea");
    quizArea.innerHTML = "<p>Loading quiz...</p>";

    var data = await loadQuiz();

    quizArea.innerHTML = "";

    data.forEach(function (q, index) {

        var div = document.createElement("div");
        var html = "<p><strong>Q" + (index + 1) + ":</strong> " + q.question + "</p>";

        q.options.forEach(function (option, i) {
            html +=
                "<label>" +
                "<input type='radio' name='q" + index + "' value='" + i + "'>" +
                option +
                "</label><br>";
        });

        div.innerHTML = html;
        quizArea.appendChild(div);
    });
}

function calculatePercentage(score, total) {
    return Math.round((score / total) * 100);
}

function calculateGrade(percentage) {

    if (percentage >= 80) return "A";
    if (percentage >= 60) return "B";
    return "C";
}

function isPassed(percentage) {
    return percentage >= 40;
}

function submitQuiz() {

    var score = 0;
    var total = questions.length;

    for (var i = 0; i < total; i++) {

        var selected = document.querySelector("input[name='q" + i + "']:checked");

        if (!selected) {
            alert("Please answer all questions");
            return;
        }

        if (parseInt(selected.value) === questions[i].correct) {
            score++;
        }
    }

    var percentage = calculatePercentage(score, total);
    var grade = calculateGrade(percentage);
    var passed = isPassed(percentage);

    saveQuizScore(percentage);

    document.getElementById("result").innerHTML =
        "<h3>Result</h3>" +
        "<p>Score: " + percentage + "%</p>" +
        "<p>Grade: " + grade + "</p>" +
        "<p>Status: " + (passed ? "Pass" : "Fail") + "</p>";
}

displayQuiz();