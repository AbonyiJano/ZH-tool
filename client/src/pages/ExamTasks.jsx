import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const ExamTasks = () => {
    const { id } = useParams();
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [editingTaskId, setEditingTaskId] = useState(null);
    const [editForm, setEditForm] = useState({});

    useEffect(() => {
        fetchTasks();
    }, [id]);

    const fetchTasks = async () => {
        try {
            const response = await apiClient.get(ENDPOINTS.TASK.GET_BY_GENERATED_ZH(id));
            setTasks(response.data);
        } catch (error) {
            console.error('Failed to fetch tasks:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleEditClick = (task) => {
        setEditingTaskId(task.id);
        setEditForm({ ...task });
    };

    const handleCancelEdit = () => {
        setEditingTaskId(null);
        setEditForm({});
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditForm(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSave = async () => {
        try {
            await apiClient.put(ENDPOINTS.TASK.UPDATE(editForm.id), editForm);
            setEditingTaskId(null);
            fetchTasks(); // Refresh list to show updated data
            alert('Task updated successfully!');
        } catch (error) {
            console.error('Failed to update task:', error);
            if (error.response && error.response.data) {
                console.error('Validation errors:', error.response.data);
                alert(`Failed to update task: ${JSON.stringify(error.response.data)}`);
            } else {
                alert('Failed to update task.');
            }
        }
    };

    if (loading) return <div className="page">Loading...</div>;

    return (
        <div className="page exam-tasks-page">
            <div className="header-actions">
                <h2>Tasks for Generated Exam #{id}</h2>
                <Link to="/generated-exams" className="btn btn-secondary">Back to List</Link>
            </div>

            <div className="tasks-grid">
                {tasks.length === 0 ? (
                    <p>No tasks found for this exam.</p>
                ) : (
                    tasks.map((task, index) => (
                        <div key={task.id || index} className="card task-card">
                            {editingTaskId === task.id ? (
                                <div className="edit-mode">
                                    <div className="form-group">
                                        <label>Task Title</label>
                                        <input
                                            name="feladatCime"
                                            value={editForm.feladatCime || ''}
                                            onChange={handleInputChange}
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label>Description</label>
                                        <textarea
                                            name="leiras"
                                            value={editForm.leiras || ''}
                                            onChange={handleInputChange}
                                            rows="3"
                                        />
                                    </div>
                                    <div className="form-row">
                                        <div className="form-group">
                                            <label>Topic</label>
                                            <input
                                                name="temakor"
                                                value={editForm.temakor || ''}
                                                onChange={handleInputChange}
                                            />
                                        </div>
                                        <div className="form-group">
                                            <label>Points</label>
                                            <input
                                                name="pontozas"
                                                value={editForm.pontozas || ''}
                                                onChange={handleInputChange}
                                            />
                                        </div>
                                        <div className="form-group">
                                            <label>Difficulty</label>
                                            <select
                                                name="nehezsegiSzint"
                                                value={editForm.nehezsegiSzint || ''}
                                                onChange={handleInputChange}
                                                className="form-control"
                                            >
                                                <option value="">Select...</option>
                                                <option value="Könnyű">Könnyű</option>
                                                <option value="Közepes">Közepes</option>
                                                <option value="Nehéz">Nehéz</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div className="form-group">
                                        <label>Example Solution</label>
                                        <textarea
                                            name="mintaMegoldas"
                                            value={editForm.mintaMegoldas || ''}
                                            onChange={handleInputChange}
                                            rows="5"
                                            className="code-input"
                                            placeholder="Enter example solution code here..."
                                        />
                                    </div>
                                    <div className="action-buttons" style={{ display: 'flex', gap: '0.5rem', marginTop: '1rem' }}>
                                        <button className="btn btn-primary" onClick={handleSave}>Save</button>
                                        <button className="btn btn-secondary" onClick={handleCancelEdit}>Cancel</button>
                                    </div>
                                </div>
                            ) : (
                                <>
                                    <div className="task-header">
                                        <h3>Task {index + 1}: {task.feladatCime}</h3>
                                        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
                                            <span className="badge">{task.nehezsegiSzint}</span>
                                            <button
                                                className="btn btn-secondary btn-sm"
                                                onClick={() => handleEditClick(task)}
                                            >
                                                Edit
                                            </button>
                                        </div>
                                    </div>
                                    <p className="task-description">{task.leiras}</p>
                                    <div className="task-meta">
                                        <span><strong>Topic:</strong> {task.temakor}</span>
                                        <span><strong>Points:</strong> {task.pontozas}</span>
                                    </div>
                                    {task.mintaMegoldas && (
                                        <div className="task-solution">
                                            <strong>Sample Solution:</strong>
                                            <pre>{task.mintaMegoldas}</pre>
                                        </div>
                                    )}
                                </>
                            )}
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};

export default ExamTasks;
