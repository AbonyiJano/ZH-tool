import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const Correction = () => {
    const location = useLocation();
    const [exams, setExams] = useState([]);
    const [students, setStudents] = useState([]);
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

        const fetchStudents = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.STUDENT.LIST);
                setStudents(response.data);
            } catch (error) {
                console.error('Failed to fetch students:', error);
            }
        };

        fetchExams();
        fetchStudents();
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
            setMessage({ type: 'success', text: 'Megoldás sikeresen beküldve javításra!' });
            setFormData({ HallgatoNeptunkod: '', GeneraltZhId: '', BekuldottMegoldas: '' });
        } catch (error) {
            console.error('Failed to submit solution:', error);
            setMessage({ type: 'error', text: 'Nem sikerült a megoldás beküldése. Ellenőrizze az azonosítókat és próbálja újra.' });
        }
    };

    return (
        <div className="page correction-page">
            <h2>ZH Javítás / Beküldés</h2>
            <div className="form-card">
                <h3>Hallgatói Megoldás Beküldése</h3>
                {message && (
                    <div className={`alert ${message.type === 'success' ? 'alert-success' : 'alert-error'}`}>
                        {message.text}
                    </div>
                )}
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label>Hallgató Neptun Kódja</label>
                        <select
                            name="HallgatoNeptunkod"
                            value={formData.HallgatoNeptunkod}
                            onChange={handleChange}
                            required
                            className="select-input"
                        >
                            <option value="">Válasszon Hallgatót</option>
                            {students.map(student => (
                                <option key={student.neptunkod} value={student.neptunkod}>
                                    {student.nev} ({student.neptunkod})
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="form-group">
                        <label>Generált ZH Azonosító</label>
                        <input
                            type="number"
                            name="GeneraltZhId"
                            value={formData.GeneraltZhId}
                            onChange={handleChange}
                            required
                            placeholder="A generált ZH példány azonosítója"
                        />
                        <small className="text-muted">Adja meg a hallgató számára generált konkrét ZH példány azonosítóját.</small>
                    </div>
                    <div className="form-group">
                        <label>Beküldött Megoldás (JSON vagy Szöveg)</label>
                        <textarea
                            name="BekuldottMegoldas"
                            value={formData.BekuldottMegoldas}
                            onChange={handleChange}
                            required
                            rows="6"
                            placeholder='{"taskId": "solution code", ...}'
                        />
                    </div>
                    <button type="submit" className="btn btn-primary">Beküldés Javításra</button>
                </form>
            </div>
        </div>
    );
};

export default Correction;
