Functional Testing Report

Student Functionality Tests

1. Enrollment Test

✅ Passed - Verified that a student can successfully enroll in a subject. The enrolled subject appears in their enrolled subjects list.

2. Drop Subject Test

✅ Passed - Verified that a student can drop a subject. The dropped subject correctly reappears in the available subjects list.

⸻

Teacher Functionality Tests

3. Create Subject Test

✅ Passed - Verified that a teacher can create a subject. The newly created subject appears in both “My Subjects” and “Available Subjects” lists.

4. Delete Subject Test

✅ Passed - Verified that deleting a subject removes it from both “My Subjects” and “Available Subjects” lists.

⸻

System-Level Tests

5. Login Test

✅ Passed - Verified that login validation works correctly for both Student and Teacher roles.

6. Data Persistence Test

✅ Passed - Verified that all changes (enrollment, subject creation, deletion) are saved to the JSON file and persist after reopening the application.

⸻

Conclusion

All functional tests have been executed successfully. The system behaves as expected for students and teachers, ensuring reliable data persistence and user authentication.