import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './pages/Home';
import CreateExam from './pages/CreateExam';
import BaseExamsList from './pages/BaseExamsList';
import BaseExamDetails from './pages/BaseExamDetails';
import StudentManagement from './pages/StudentManagement';
import Correction from './pages/Correction';
import GeneratedExamsList from './pages/GeneratedExamsList';
import ExamTasks from './pages/ExamTasks';
import CorrectionsList from './pages/CorrectionsList';
import SolutionDetails from './pages/SolutionDetails';
import './App.css';

function App() {
  return (
    <Router>
      <div className="app">
        <Navbar />
        <main className="main-content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/create-exam" element={<CreateExam />} />
            <Route path="/base-exams" element={<BaseExamsList />} />
            <Route path="/base-exams/:id" element={<BaseExamDetails />} />
            <Route path="/students" element={<StudentManagement />} />
            <Route path="/correction" element={<Correction />} />
            <Route path="/generated-exams" element={<GeneratedExamsList />} />
            <Route path="/generated-exams/:id/tasks" element={<ExamTasks />} />
            <Route path="/corrections" element={<CorrectionsList />} />
            <Route path="/solutions/:id" element={<SolutionDetails />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
