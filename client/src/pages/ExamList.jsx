import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const ExamList = () => {
    const [exams, setExams] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchExams = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.EXAM.LIST);
                setExams(response.data);
            } catch (error) {
                console.error('Failed to fetch exams:', error);
            }
        };

        fetchExams();
    }, []);

    const handleStartExam = async (examId) => {
        try {
            const response = await apiClient.post(ENDPOINTS.EXAM.GENERATE(examId));
            const generatedZh = response.data;
            navigate(`/exam/${generatedZh.id}`);
        } catch (error) {
            console.error('Failed to start exam:', error);
            alert('Failed to start exam. Please try again.');
        }
    };

    return (
        <div className="page exam-list-page">
            <h2>Available Exams</h2>
            <div className="exam-grid">
                {exams.length === 0 ? (
                    <p>No exams available.</p>
                ) : (
                    exams.map((exam) => (
                        <div key={exam.id} className="card exam-card">
                            <h3>{exam.nev || 'Unnamed Exam'}</h3>
                            <p>Difficulty: {exam.nehezseg}</p>
                            <p>Language: {exam.programozasiNyelv}</p>
                            <button
                                className="btn btn-primary"
                                onClick={() => handleStartExam(exam.id)}
                            >
                                Start Exam
                            </button>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};

export default ExamList;
