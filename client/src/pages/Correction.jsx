import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const Correction = () => {
    const location = useLocation();
    const [exams, setExams] = useState([]);
    const [formData, setFormData] = useState({
        HallgatoNeptunkod: '',
        GeneraltZhId: '',
        BekuldottMegoldas: ''
    });
    const [message, setMessage] = useState(null);

    useEffect(() => {
        if (location.state?.generaltZhId) {
            setFormData(prev => ({
                ...prev,
                GeneraltZhId: location.state.generaltZhId
            }));
        }

        const fetchExams = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.EXAM.LIST);
                setExams(response.data);
            } catch (error) {
                console.error('Failed to fetch exams:', error);
            }
        };
        fetchExams();
    }, [location.state]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage(null);
        try {
            await apiClient.post(ENDPOINTS.SOLUTION.SUBMIT, {
                ...formData,
                GeneraltZhId: parseInt(formData.GeneraltZhId)
            });
            setMessage({ type: 'success', text: 'Solution submitted for correction successfully!' });
            setFormData({ HallgatoNeptunkod: '', GeneraltZhId: '', BekuldottMegoldas: '' });
        } catch (error) {
            console.error('Failed to submit solution:', error);
            setMessage({ type: 'error', text: 'Failed to submit solution. Check IDs and try again.' });
        }
    };

    return (
        <div className="page correction-page">
            <h2>Exam Correction / Submission</h2>
            <div className="form-card">
                <h3>Submit Student Solution</h3>
                {message && (
                    <div className={`alert ${message.type === 'success' ? 'alert-success' : 'alert-error'}`}>
                        {message.text}
                    </div>
                )}
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label>Student Neptun Code</label>
                        <input
                            name="HallgatoNeptunkod"
                            value={formData.HallgatoNeptunkod}
                            onChange={handleChange}
                            required
                            placeholder="e.g., ABC123"
                        />
                    </div>
                    <div className="form-group">
                        <label>Generated Exam ID</label>
                        <input
                            type="number"
                            name="GeneraltZhId"
                            value={formData.GeneraltZhId}
                            onChange={handleChange}
                            required
                            placeholder="ID of the generated exam instance"
                        />
                        <small className="text-muted">Enter the ID of the specific exam instance generated for the student.</small>
                    </div>
                    <div className="form-group">
                        <label>Submitted Solution (JSON or Text)</label>
                        <textarea
                            name="BekuldottMegoldas"
                            value={formData.BekuldottMegoldas}
                            onChange={handleChange}
                            required
                            rows="6"
                            placeholder='{"taskId": "solution code", ...}'
                        />
                    </div>
                    <button type="submit" className="btn btn-primary">Submit for Correction</button>
                </form>
            </div>
        </div>
    );
};

export default Correction;
