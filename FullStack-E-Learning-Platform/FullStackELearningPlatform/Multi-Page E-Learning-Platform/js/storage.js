function getCompletedCourses() {

    var completed = localStorage.getItem("completedCourses");

    if (completed) {
        var parsed = JSON.parse(completed);

        // Remove duplicates
        return parsed.filter(function (value, index, self) {
            return self.indexOf(value) === index;
        });
    }

    return [];
}

function saveCompletedCourse(courseName) {

    var completed = getCompletedCourses();

    if (!completed.includes(courseName)) {
        completed.push(courseName);
        localStorage.setItem("completedCourses", JSON.stringify(completed));
    }
}

function getQuizScore() {
    return localStorage.getItem("quizScore");
}

function saveQuizScore(score) {
    localStorage.setItem("quizScore", score);
}