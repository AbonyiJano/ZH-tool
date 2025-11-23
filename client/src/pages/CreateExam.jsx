import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const CreateExam = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        Nev: '',
        MintaZh: '',
        Tematika: '',
        TemakorLeiras: '',
        FeladatokSzama: 1,
        ProgramozasiNyelv: '',
        Nehezseg: ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: name === 'FeladatokSzama' ? parseInt(value) : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await apiClient.post(ENDPOINTS.EXAM.CREATE, formData);
            alert('Exam created successfully!');
            navigate('/');
        } catch (error) {
            console.error('Failed to create exam:', error);
            alert('Failed to create exam. Please check your inputs.');
        }
    };

    return (
        <div className="page create-exam-page">
            <h2>Create New Exam</h2>
            <form onSubmit={handleSubmit} className="form-card">
                <div className="form-group">
                    <label>Exam Name</label>
                    <input name="Nev" value={formData.Nev} onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label>Sample ZH (Optional)</label>
                    <textarea name="MintaZh" value={formData.MintaZh} onChange={handleChange} rows="3" />
                </div>
                <div className="form-group">
                    <label>Topic / Theme</label>
                    <input name="Tematika" value={formData.Tematika} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Topic Description</label>
                    <textarea name="TemakorLeiras" value={formData.TemakorLeiras} onChange={handleChange} rows="3" />
                </div>
                <div className="form-row">
                    <div className="form-group">
                        <label>Number of Tasks</label>
                        <input
                            type="number"
                            name="FeladatokSzama"
                            value={formData.FeladatokSzama}
                            onChange={handleChange}
                            min="1"
                            max="100"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label>Programming Language</label>
                        <input name="ProgramozasiNyelv" value={formData.ProgramozasiNyelv} onChange={handleChange} />
                    </div>
                    <div className="form-group">
                        <label>Difficulty</label>
                        <select name="Nehezseg" value={formData.Nehezseg} onChange={handleChange}>
                            <option value="">Select Difficulty</option>
                            <option value="Könnyű">Easy</option>
                            <option value="Közepes">Medium</option>
                            <option value="Nehéz">Hard</option>
                        </select>
                    </div>
                </div>
                <button type="submit" className="btn btn-primary">Create Exam</button>
            </form>
        </div>
    );
};

export default CreateExam;
