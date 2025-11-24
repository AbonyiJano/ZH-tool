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
import SolutionsList from './pages/SolutionsList';
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
            <Route path="/uj-zh" element={<CreateExam />} />
            <Route path="/alap-zhk" element={<BaseExamsList />} />
            <Route path="/alap-zhk/:id" element={<BaseExamDetails />} />
            <Route path="/hallgatok" element={<StudentManagement />} />
            <Route path="/javitas" element={<Correction />} />
            <Route path="/generalt-zhk" element={<GeneratedExamsList />} />
            <Route path="/generalt-zhk/:id/feladatok" element={<ExamTasks />} />
            <Route path="/javitasok" element={<SolutionsList />} />
            <Route path="/megoldasok/:id" element={<SolutionDetails />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
