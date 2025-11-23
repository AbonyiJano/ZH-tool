import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const BaseExamsList = () => {
    const [exams, setExams] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchExams = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.EXAM.LIST);
                setExams(response.data);
            } catch (error) {
                console.error('Failed to fetch exams:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchExams();
    }, []);

    const handleGenerate = async (examId) => {
        try {
            await apiClient.post(ENDPOINTS.EXAM.GENERATE(examId));
            alert('New exam instance generated successfully!');
        } catch (error) {
            console.error('Failed to generate exam:', error);
            alert('Failed to generate exam.');
        }
    };

    if (loading) return <div className="page">Loading...</div>;

    return (
        <div className="page base-exams-page">
            <h2>Base Exams (Templates)</h2>
            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {exams.map((exam) => (
                            <tr key={exam.id}>
                                <td>{exam.id}</td>
                                <td>{exam.nev}</td>
                                <td>
                                    <div style={{ display: 'flex', gap: '0.5rem' }}>
                                        <Link to={`/base-exams/${exam.id}`} className="btn btn-secondary btn-sm">
                                            View Details
                                        </Link>
                                        <button
                                            className="btn btn-primary btn-sm"
                                            onClick={() => handleGenerate(exam.id)}
                                        >
                                            Generate Instance
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                {exams.length === 0 && <p className="text-center mt-4">No base exams found.</p>}
            </div>
        </div>
    );
};

export default BaseExamsList;
