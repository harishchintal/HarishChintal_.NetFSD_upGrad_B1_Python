const { calculatePercentage, calculateGrade, isPassed } = require('../js/quiz');

test("Percentage calculation works", () => {
    expect(calculatePercentage(8, 10)).toBe(80);
});

test("Grade calculation works", () => {
    expect(calculateGrade(85)).toBe("A");
});

test("Pass logic works", () => {
    expect(isPassed(35)).toBe(false);
});