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
            alert('ZH sikeresen létrehozva!');
            navigate('/');
        } catch (error) {
            console.error('Failed to create exam:', error);
            alert('Nem sikerült létrehozni a ZH-t. Kérjük, ellenőrizze a bemeneteket.');
        }
    };

    return (
        <div className="page create-exam-page">
            <h2>Új ZH Sablon Létrehozása</h2>
            <form onSubmit={handleSubmit} className="form-card">
                <div className="form-group">
                    <label>ZH Neve</label>
                    <input name="Nev" value={formData.Nev} onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label>Minta ZH (Opcionális)</label>
                    <textarea name="MintaZh" value={formData.MintaZh} onChange={handleChange} rows="3" />
                </div>
                <div className="form-group">
                    <label>Tematika</label>
                    <input name="Tematika" value={formData.Tematika} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Témakör Leírása</label>
                    <textarea name="TemakorLeiras" value={formData.TemakorLeiras} onChange={handleChange} rows="3" />
                </div>
                <div className="form-row">
                    <div className="form-group">
                        <label>Feladatok Száma</label>
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
                        <label>Programozási Nyelv</label>
                        <input name="ProgramozasiNyelv" value={formData.ProgramozasiNyelv} onChange={handleChange} />
                    </div>
                    <div className="form-group">
                        <label>Nehézség</label>
                        <select name="Nehezseg" value={formData.Nehezseg} onChange={handleChange}>
                            <option value="">Válasszon Nehézséget</option>
                            <option value="Könnyű">Könnyű</option>
                            <option value="Közepes">Közepes</option>
                            <option value="Nehéz">Nehéz</option>
                        </select>
                    </div>
                </div>
                <button type="submit" className="btn btn-primary">ZH Létrehozása</button>
            </form>
        </div>
    );
};

export default CreateExam;
