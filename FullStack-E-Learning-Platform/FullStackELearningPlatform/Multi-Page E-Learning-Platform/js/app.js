document.addEventListener("DOMContentLoaded", function () {

    highlightActiveNav();

    // ================= DASHBOARD PAGE =================
    if (document.getElementById("totalCourses")) {
        updateDashboard();
    }

    // ================= COURSES PAGE =================
    if (document.getElementById("courseContainer")) {

        var container = document.getElementById("courseContainer");
        var tableBody = document.getElementById("courseTable");

        courses.forEach(function (course) {

            var card = document.createElement("div");
            card.className = "card";

            var lessonHtml = "<ol>";
            course.lessons.forEach(function (lesson) {
                lessonHtml += "<li>" + lesson + "</li>";
            });
            lessonHtml += "</ol>";

            // card.innerHTML =
            //     "<h3>" + course.name + "</h3>" +
            //     lessonHtml +
            //     "<button onclick=\"markComplete('" + course.name + "')\">Mark Complete</button>";

            card.innerHTML =
    "<h3>" + course.name + "</h3>" +
    "<p><strong>Duration:</strong> " + course.duration + "</p>" +
    lessonHtml +
    "<button onclick=\"markComplete('" + course.name + "')\">Mark Complete</button>";

            container.appendChild(card);

            var row = document.createElement("tr");
            // row.innerHTML =
            //     "<td>" + course.name + "</td>" +
            //     "<td>" + course.lessons.length + "</td>";

            row.innerHTML =
    "<td>" + course.name + "</td>" +
    "<td>" + course.lessons.length + "</td>" +
    "<td>" + course.duration + "</td>";

            tableBody.appendChild(row);
        });
    }

    // ================= PROFILE PAGE =================
    if (document.getElementById("profileTotalCourses")) {

        var total = courses.length;
        var completed = getCompletedCourses();
        var score = getQuizScore();

        // Profile summary
        document.getElementById("profileTotalCourses").innerText = total;
        document.getElementById("profileCompletedCourses").innerText = completed.length;

        var percent = 0;

        if (total > 0) {
            percent = Math.round((completed.length / total) * 100);
        }

        document.getElementById("profileProgress").innerText = percent + "%";

        // Quiz Score
        document.getElementById("storedScore").innerText =
            score ? score + "%" : "Not Attempted";

        // Completed Course List
        var list = document.getElementById("completedCoursesList");

        completed.forEach(function (courseName) {
            var li = document.createElement("li");
            li.innerText = courseName;
            list.appendChild(li);
        });
    }

});


// ================= MARK COURSE COMPLETE =================
function markComplete(courseName) {

    saveCompletedCourse(courseName);
    alert("Course marked as completed");

    updateDashboard();
}


// ================= UPDATE DASHBOARD =================
function updateDashboard() {

    var total = courses.length;
    var completed = getCompletedCourses();

    document.getElementById("totalCourses").innerText = total;

    // Prevent showing more than total
    var safeCompletedCount =
        completed.length > total ? total : completed.length;

    document.getElementById("completedCount").innerText = safeCompletedCount;

    var percent = 0;

    if (total > 0) {
        percent = Math.round((safeCompletedCount / total) * 100);
    }

    document.getElementById("progressBar").value = percent;
    document.getElementById("progressText").innerText = percent + "% Completed";
}


// ================= ACTIVE NAVIGATION =================
function highlightActiveNav() {

    var links = document.querySelectorAll("nav a");

    links.forEach(function (link) {
        if (link.href === window.location.href) {
            link.classList.add("active");
        }
    });
}