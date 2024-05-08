
    var questions = document.querySelectorAll('.question');
    var questionNumbers = document.querySelectorAll('.questionNumberCell');
    var currentQuestionIndex = 0;

    // Array to track which questions have been attempted
    var attemptedQuestions = Array.from({ length: questions.length }, () => false);

    // Function to update the attemptedQuestions array when a question is attempted
    function updateAttemptedQuestions(questionIndex, attempted) {
        attemptedQuestions[questionIndex] = attempted;
    }

    // Function to highlight the attempted question number
    function highlightAttemptedQuestion() {
        questionNumbers.forEach(function (number, index) {
            if (attemptedQuestions[index]) {
                number.classList.add('attempted');
            } else {
                number.classList.remove('attempted');
            }
        });
    }

    // Function to show or hide buttons based on current question index
    function toggleButtons() {
        if (currentQuestionIndex === 0) {
            document.getElementById('prevButton').style.display = 'none';
        } else {
            document.getElementById('prevButton').style.display = 'inline-block';
        }

        if (currentQuestionIndex === questions.length - 1) {
            document.getElementById('nextButton').style.display = 'none';
            document.getElementById('submitButton').style.display = 'inline-block';
        } else {
            document.getElementById('nextButton').style.display = 'inline-block';
            document.getElementById('submitButton').style.display = 'none';
        }
    }

    // Function to show the current question and toggle buttons
    function showCurrentQuestion() {
        questions.forEach(function (question, index) {
            if (index === currentQuestionIndex) {
                question.style.display = 'block';
            } else {
                question.style.display = 'none';
            }
        });
        toggleButtons();
    }

    // Handle "Next" button click
    document.getElementById('nextButton').addEventListener('click', function () {
        if (currentQuestionIndex < questions.length - 1) {
            // Update attempted questions before moving to the next question
            var currentQuestion = questions[currentQuestionIndex];
            var selectedOption = currentQuestion.querySelector('input[name="question_' + currentQuestion.dataset.questionnaireId + '"]:checked');
            if (selectedOption) {
                updateAttemptedQuestions(currentQuestionIndex, true);
                highlightAttemptedQuestion();
            }

            // Move to the next question
            currentQuestionIndex++;
            showCurrentQuestion();
        }
    });

    // Handle "Previous" button click
    document.getElementById('prevButton').addEventListener('click', function () {
        if (currentQuestionIndex > 0) {
            currentQuestionIndex--;
            showCurrentQuestion();
        }
    });

    // Handle clicking on a question number
    questionNumbers.forEach(function (number, index) {
        number.addEventListener('click', function () {
            currentQuestionIndex = index;
            showCurrentQuestion();
        });
    });

    // Handle radio button change
    questions.forEach(function (question, index) {
        question.addEventListener('change', function () {
            updateAttemptedQuestions(index, true);
            highlightAttemptedQuestion();
        });
    });

    // Show the first question initially
    showCurrentQuestion();

    // Function to show the previous question
    function showPreviousQuestion() {
        if (currentQuestionIndex > 0) {
            currentQuestionIndex--;
            showCurrentQuestion();
        }
    }

    // Handle "Submit" button click
    document.getElementById('submitButton').addEventListener('click', function () {

        var selectedOptionsArray = [];
        questions.forEach(function (question) {
            var selectedOption = question.querySelector('input[name="question_' + question.dataset.questionnaireId + '"]:checked');
            if (selectedOption) {
                var questionId = parseInt(question.dataset.questionnaireId);
                selectedOptionsArray.push({ questionId: questionId, selectedOption: selectedOption.value });
            }
        });

        fetch('@Url.Action("Submit", "ExamMasters")', {
            method: 'POST',
            body: JSON.stringify(selectedOptionsArray),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(function (response) {
                // Handle response from the server
            })
            .catch(function (error) {
                // Handle error
            });
    });

    // Function to enter fullscreen mode
    function enterFullscreen() {
        var element = document.documentElement;
        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.webkitRequestFullscreen) { /* Safari */
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) { /* IE11 */
            element.msRequestFullscreen();
        }
    }

    // Event listener for start exam button
    document.getElementById('startExamButton').addEventListener('click', function () {
        // Show the exam UI
        document.getElementById('examUI').style.display = 'block';
        // Enter fullscreen mode
        enterFullscreen();
        // Hide the "Start Exam" button and rules modal
        document.getElementById('startExamContainer').style.display = 'none';
        document.getElementById('rulesModal').style.display = 'none';
    });




    // Access timer value
    var timerInSeconds = @Model.Item2;

    // Function to update timer display
    function updateTimerDisplay() {
        var minutes = Math.floor(timerInSeconds / 60);
        var seconds = timerInSeconds % 60;
        document.getElementById('timer').innerText = pad(minutes) + ':' + pad(seconds);
    }

    // Function to pad single digit numbers with leading zero
    function pad(number) {
        return (number < 10 ? '0' : '') + number;
    }

    // Update timer display initially
    updateTimerDisplay();

    // Function to update timer every second
    var timerInterval = setInterval(function () {
        timerInSeconds--;
        if (timerInSeconds >= 0) {
            updateTimerDisplay();
        } else {
            // Timer expired, submit the exam
            clearInterval(timerInterval);
            document.getElementById('submitButton').click(); // Trigger submit button click
        }
    }, 1000);



    // Prevent user from reloading or leaving the page
    window.onbeforeunload = function () {
        // Exit fullscreen mode before leaving the page
        var fullscreenElement = document.fullscreenElement || document.webkitFullscreenElement || document.msFullscreenElement;
        if (fullscreenElement) {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.webkitExitFullscreen) { /* Safari */
                document.webkitExitFullscreen();
            } else if (document.msExitFullscreen) { /* IE11 */
                document.msExitFullscreen();
            }
        }
        return "Are you sure you want to leave this page? Your progress will be lost.";
    };



    // Function to enter fullscreen mode
    function enterFullscreen() {
        var element = document.documentElement;
        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.webkitRequestFullscreen) { /* Safari */
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) { /* IE11 */
            element.msRequestFullscreen();
        }
    }

    // Event listener for "Start Exam" button
    document.getElementById('startExamButton').addEventListener('click', function () {
        // Hide the terms container
        document.getElementById('termsContainer').style.display = 'none';
        // Show the exam UI
        document.getElementById('examUI').style.display = 'block';
        // Enter fullscreen mode
        enterFullscreen();
        // Prevent default behavior of F5 and other keyboard shortcuts
        document.addEventListener('keydown', preventDefaultKeys);
    });

    // Function to prevent default behavior of keys
    function preventDefaultKeys(event) {
        if (event.key === 'F5' || (event.key === 'r' && event.ctrlKey) || (event.key === 'R' && event.ctrlKey)) {
            event.preventDefault();
        }
    }




    // Function to exit fullscreen mode
    function exitFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) { /* Safari */
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { /* IE11 */
            document.msExitFullscreen();
        }
    }

    // Event listener for "Submit" button click
    document.getElementById('submitButton').addEventListener('click', function () {
        // Exit fullscreen mode
        exitFullscreen();
    });

    // Function to stop the timer and submit the exam
    function submitExam() {
        clearInterval(timerInterval);
        document.getElementById('submitButton').click(); // Trigger submit button click
    }

    // Update timer display initially
    updateTimerDisplay();

    // Function to update timer every second
    var timerInterval = setInterval(function () {
        timerInSeconds--;
        if (timerInSeconds >= 0) {
            updateTimerDisplay();
        } else {
            // Timer expired, submit the exam
            submitExam();
        }
    }, 1000);


    // Function to stop the timer and submit the exam
    function submitExam() {
        clearInterval(timerInterval);
        document.getElementById('submitButton').click(); // Trigger submit button click
        // Clear the onbeforeunload event listener
        window.onbeforeunload = null;
    }

    // Function to show the "Thank You" message and stop the timer
    function showThankYouMessage() {
        clearInterval(timerInterval);
        document.getElementById('thankYouMessage').style.display = 'block';
        // Clear the onbeforeunload event listener
        window.onbeforeunload = null;
    }

    // Function to stop the timer and submit the exam when the timer ends
    function timerExpired() {
        showThankYouMessage();
        submitExam();
    }

    // Handle "Submit" button click
    document.getElementById('submitButton').addEventListener('click', function () {
        // Exit fullscreen mode
        exitFullscreen();
        // Clear the onbeforeunload event listener
        window.onbeforeunload = null;
        // Redirect to the index page
        window.location.href = '@Url.Action("Index", "ExamMasters")'; // Change the URL to match your controller and action
    });
