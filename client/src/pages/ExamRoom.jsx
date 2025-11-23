import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const ExamRoom = () => {
    const { id } = useParams(); // generatedZhId
    const navigate = useNavigate();
    const [tasks, setTasks] = useState([]);
    const [solutions, setSolutions] = useState({});
    const [submitting, setSubmitting] = useState(false);

    useEffect(() => {
        const fetchTasks = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.TASK.GET_BY_GENERATED_ZH(id));
                setTasks(response.data);
            } catch (error) {
                console.error('Failed to fetch tasks:', error);
            }
        };

        fetchTasks();
    }, [id]);

    const handleSolutionChange = (taskId, value) => {
        setSolutions(prev => ({
            ...prev,
            [taskId]: value
        }));
    };

    const handleSubmit = async () => {
        if (!window.confirm('Are you sure you want to submit your solutions?')) return;

        setSubmitting(true);
        const user = JSON.parse(localStorage.getItem('user'));

        if (!user || !user.neptunkod) {
            alert('User not found. Please login again.');
            navigate('/login');
            return;
        }

        try {
            // We submit all solutions as one JSON string or individually?
            // The backend expects ONE "BekuldottMegoldas" string.
            // So we should probably wrap all answers in a JSON object.
            const solutionData = {
                HallgatoNeptunkod: user.neptunkod,
                GeneraltZhId: parseInt(id),
                BekuldottMegoldas: JSON.stringify(solutions)
            };

            await apiClient.post(ENDPOINTS.SOLUTION.SUBMIT, solutionData);
            alert('Solutions submitted successfully!');
            navigate('/exams');
        } catch (error) {
            console.error('Failed to submit solutions:', error);
            alert('Failed to submit solutions. Please try again.');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="page exam-room-page">
            <h2>Exam Room</h2>
            <div className="tasks-container">
                {tasks.map((task, index) => (
                    <div key={task.id || index} className="card task-card">
                        <h3>Task {index + 1}: {task.feladatCime}</h3>
                        <p className="task-description">{task.leiras}</p>
                        <div className="task-meta">
                            <span>Points: {task.pontozas}</span>
                            <span>Topic: {task.temakor}</span>
                        </div>
                        <div className="solution-area">
                            <label>Your Solution:</label>
                            <textarea
                                rows="5"
                                value={solutions[task.id] || ''}
                                onChange={(e) => handleSolutionChange(task.id, e.target.value)}
                                placeholder="Type your code or answer here..."
                            />
                        </div>
                    </div>
                ))}
            </div>
            <div className="exam-actions">
                <button
                    className="btn btn-primary"
                    onClick={handleSubmit}
                    disabled={submitting}
                >
                    {submitting ? 'Submitting...' : 'Submit Exam'}
                </button>
            </div>
        </div>
    );
};

export default ExamRoom;
