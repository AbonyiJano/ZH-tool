import React, { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const BaseExamDetails = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [exam, setExam] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchExam = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.EXAM.GET_BY_ID(id));
                setExam(response.data);
            } catch (error) {
                console.error('Failed to fetch exam details:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchExam();
    }, [id]);

    const handleGenerate = async () => {
        try {
            await apiClient.post(ENDPOINTS.EXAM.GENERATE(id));
            alert('New exam instance generated successfully!');
            navigate('/generated-exams');
        } catch (error) {
            console.error('Failed to generate exam:', error);
            alert('Failed to generate exam.');
        }
    };

    if (loading) return <div className="page">Loading...</div>;
    if (!exam) return <div className="page">Exam not found.</div>;

    return (
        <div className="page base-exam-details-page">
            <div className="header-actions">
                <h2>Base Exam Details #{exam.id}</h2>
                <Link to="/base-exams" className="btn btn-secondary">Back to List</Link>
            </div>

            <div className="card">
                <div className="form-group">
                    <label>Exam Name</label>
                    <div className="static-value">{exam.nev}</div>
                </div>
                <div className="form-group">
                    <label>Sample Exam ID</label>
                    <div className="static-value">{exam.mintaZh}</div>
                </div>
                <div className="form-group">
                    <label>Topic</label>
                    <div className="static-value">{exam.tematika}</div>
                </div>
                <div className="form-group">
                    <label>Description</label>
                    <div className="static-value">{exam.temakorLeiras}</div>
                </div>
                <div className="form-row">
                    <div className="form-group">
                        <label>Task Count</label>
                        <div className="static-value">{exam.feladatokSzama}</div>
                    </div>
                    <div className="form-group">
                        <label>Language</label>
                        <div className="static-value">{exam.programozasiNyelv}</div>
                    </div>
                    <div className="form-group">
                        <label>Difficulty</label>
                        <div className="static-value">{exam.nehezseg}</div>
                    </div>
                </div>

                <div className="mt-4">
                    <button onClick={handleGenerate} className="btn btn-primary">
                        Generate New Instance
                    </button>
                </div>
            </div>
        </div>
    );
};

export default BaseExamDetails;
