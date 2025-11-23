import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const StudentLogin = () => {
    const [neptun, setNeptun] = useState('');
    const [name, setName] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await apiClient.post(ENDPOINTS.STUDENT.LOGIN, {
                Neptunkod: neptun,
                Nev: name
            });
            console.log('Login success:', response.data);
            localStorage.setItem('user', JSON.stringify(response.data));
            navigate('/exams');
        } catch (error) {
            console.error('Login failed:', error);
            alert('Login failed. Please check your credentials.');
        }
    };

    return (
        <div className="page login-page">
            <h2>Student Login</h2>
            <form onSubmit={handleLogin} className="login-form">
                <div className="form-group">
                    <label>Neptun Code</label>
                    <input
                        type="text"
                        value={neptun}
                        onChange={(e) => setNeptun(e.target.value)}
                        required
                    />
                </div>
                <div className="form-group">
                    <label>Name</label>
                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                    />
                </div>
                <button type="submit" className="btn btn-primary">Login</button>
            </form>
        </div>
    );
};

export default StudentLogin;
