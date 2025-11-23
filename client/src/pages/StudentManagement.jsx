import React, { useState } from 'react';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const StudentManagement = () => {
    const [formData, setFormData] = useState({
        Neptunkod: '',
        Nev: ''
    });
    const [message, setMessage] = useState(null);

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
            await apiClient.post(ENDPOINTS.STUDENT.LOGIN, formData); // Using LOGIN endpoint as it's the same as Create/Register
            setMessage({ type: 'success', text: 'Student registered successfully!' });
            setFormData({ Neptunkod: '', Nev: '' });
        } catch (error) {
            console.error('Failed to register student:', error);
            setMessage({ type: 'error', text: 'Failed to register student. Neptun code might already exist.' });
        }
    };

    return (
        <div className="page student-management-page">
            <h2>Student Management</h2>
            <div className="form-card">
                <h3>Register New Student</h3>
                {message && (
                    <div className={`alert ${message.type === 'success' ? 'alert-success' : 'alert-error'}`}>
                        {message.text}
                    </div>
                )}
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label>Neptun Code</label>
                        <input
                            name="Neptunkod"
                            value={formData.Neptunkod}
                            onChange={handleChange}
                            required
                            minLength="6"
                            maxLength="6"
                            placeholder="e.g., ABC123"
                        />
                    </div>
                    <div className="form-group">
                        <label>Name</label>
                        <input
                            name="Nev"
                            value={formData.Nev}
                            onChange={handleChange}
                            required
                            placeholder="Student Name"
                        />
                    </div>
                    <button type="submit" className="btn btn-primary">Register Student</button>
                </form>
            </div>
        </div>
    );
};

export default StudentManagement;
