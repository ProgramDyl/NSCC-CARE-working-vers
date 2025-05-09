import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';
import axios from 'axios';
import AssessmentsCard from '../components/profile-components/AssessmentsCard';
import '../css/assessment_styles.css';
import { Snackbar, Alert } from '@mui/material';


const PatientProgressNote = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [answers, setAnswers] = useState({});
    const [initialAnswers, setInitialAnswers] = useState({});

    const APIHOST = import.meta.env.VITE_API_URL;

    //notifications
    const [snackbar, setSnackbar] = useState({
                open: false,
                message: '',
                severity: 'info'
    });

    // Function to get current date-time
    const getCurrentDateTime = () => {
        const now = new Date();
        const offset = now.getTimezoneOffset() * 60000;
        const localISOTime = new Date(now - offset).toISOString().slice(0, 16);
        return localISOTime;
    };

    // Load data from localStorage on component mount
    useEffect(() => {
        const savedData = localStorage.getItem(`patient-progressnote-${id}`);
        if (savedData) {
            const parsed = JSON.parse(savedData);
            setAnswers(parsed);
            setInitialAnswers(parsed);
        } else {
            const defaultState = {
                timestamp: getCurrentDateTime()
            };
            setAnswers(defaultState);
            setInitialAnswers(defaultState);
            fetchPatientData();
        }
    }, [id]);

    const fetchPatientData = async () => {
        try {
            const response = await axios.get(`${APIHOST}/api/patients/nurse/patient/${id}/progressnote`);
            console.log('Response:', response.data);
            setAnswers(response.data);
            setInitialAnswers(response.data);
        } catch (error) {
            console.error('Error fetching patient:', error);
        }
    };

    // Handle field changes
    const handleAnswerChange = (question, answer) => {
        setAnswers(prevAnswers => ({
            ...prevAnswers,
            [question]: answer
        }));
    };

    // Save function for the Save button
    const handleSave = () => {
        try {
            localStorage.setItem(`patient-progressnote-${id}`, JSON.stringify(answers));
            setInitialAnswers(answers);
            setSnackbar({
                open: true,
                message: 'Patient record saved successfully!',
                severity: 'success'
            });
        } catch (error) {
            console.error('Error saving data:', error);
            setSnackbar({
                open: true,
                message: 'Error: Failed to save patient data.',
                severity: 'error'
            });
        }
    };

    // Check if there are any changes
    const isDirty = () =>
        JSON.stringify(answers) !== JSON.stringify(initialAnswers);

    return (
        <div className="container mt-4 d-flex assessment-page">
            {/* Sidebar */}
            <AssessmentsCard />

            {/* Content */}
            <div className="ms-4 flex-fill">
                {/* Title & Buttons */}
                <div className="d-flex justify-content-between align-items-center mb-4 assessment-header">
                    <text>Progress Note</text>
                    <div className="d-flex gap-2">
                        <Button
                            variant="primary"
                            onClick={() => navigate(`/api/patients/${id}`)}
                        >
                            Go Back to Profile
                        </Button>
                        <Button
                            onClick={handleSave}
                            disabled={!isDirty()}
                            variant={isDirty() ? 'success' : 'secondary'}
                            style={{
                                opacity: isDirty() ? 1 : 0.5,
                                cursor: isDirty() ? 'pointer' : 'not-allowed',
                                border: 'none',
                                backgroundColor: isDirty() ? '#198754' : '#e0e0e0',
                                color: isDirty() ? 'white' : '#777',
                                pointerEvents: isDirty() ? 'auto' : 'none'
                            }}
                        >
                            {isDirty() ? 'Save' : 'No Changes'}
                        </Button>
                    </div>
                </div>

                {/* Date */}
                <Card className="mt-4 gradient-background">
                    <Card.Body>
                        <Form>
                            <Form.Group className="mb-3">
                                <Form.Label>Date</Form.Label>
                                <Form.Control
                                    type="datetime-local"
                                    value={answers.timestamp || getCurrentDateTime()}
                                    onChange={(e) => handleAnswerChange('timestamp', e.target.value)}
                                />
                            </Form.Group>
                        </Form>
                    </Card.Body>
                </Card>

                {/* Progress Notes */}
                <Card className="mt-4 gradient-background">
                    <Card.Body>
                        <Form>
                            <Form.Group className="mb-3">
                                <Form.Label>Progress Notes</Form.Label>
                                <Form.Control
                                    as="textarea"
                                    rows={10}
                                    value={answers.note || ''}
                                    onChange={(e) => handleAnswerChange('note', e.target.value)}
                                    placeholder="Enter detailed progress notes here..."
                                />
                            </Form.Group>
                        </Form>
                    </Card.Body>
                </Card>
            </div>
        </div>
    );
};

export default PatientProgressNote;
