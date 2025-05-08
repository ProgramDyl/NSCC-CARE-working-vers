import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';
import axios from 'axios';
import AssessmentsCard from '../components/profile-components/AssessmentsCard';
import '../css/assessment_styles.css';
import { Snackbar, Alert } from '@mui/material';


const PatientBehaviour = () => {
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

    // Load data from localStorage on component mount
    useEffect(() => {
        const savedData = localStorage.getItem(`patient-behaviour-${id}`);
        if (savedData) {
            const parsed = JSON.parse(savedData);
            setAnswers(parsed);
            setInitialAnswers(parsed);
        } else {
            fetchPatientData();
        }
    }, [id]);

    const fetchPatientData = async () => {
        try {
            const response = await axios.get(`${APIHOST}/api/patients/nurse/patient/${id}/behaviour`);
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
            // Save to localStorage
            localStorage.setItem(`patient-behaviour-${id}`, JSON.stringify(answers));
            // Update initial state
            setInitialAnswers(answers);
            // Show success message
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
        <div className="container mt-4 d-flex">
            {/* Sidebar */}
            <AssessmentsCard />

            {/* Content */}
            <div className="ms-4 flex-fill assessment-page">
                {/* Title & Buttons */}
                <div className="d-flex justify-content-between align-items-center mb-4 assessment-header">
                    <text>Behaviour</text>
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

                {/* Behaviour Notes */}
                <Card className="mt-4 gradient-background">
                    <Card.Body>
                        <Form>
                            <Form.Group className="mb-3">
                                <Form.Label>Behaviour Notes:</Form.Label>
                                <Form.Control
                                    as="textarea"
                                    rows={3}
                                    value={answers.report || ''}
                                    onChange={(e) =>
                                        handleAnswerChange('report', e.target.value)
                                    }
                                />
                            </Form.Group>
                        </Form>
                    </Card.Body>
                </Card>
            </div>
            <Snackbar
                open={snackbar.open}
                autoHideDuration={6000}
                onClose={() => setSnackbar(prev => ({...prev, open: false}))}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
            >
                <Alert 
                    onClose={() => setSnackbar(prev => ({...prev, open: false}))}
                    severity={snackbar.severity}
                    sx={{ width: '100%' }}
                >
                    {snackbar.message}
                </Alert>
            </Snackbar>
        </div>
    );
};

export default PatientBehaviour;
