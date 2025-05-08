import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';
import axios from 'axios';
import AssessmentsCard from '../components/profile-components/AssessmentsCard';
import '../css/assessment_styles.css';
import { Snackbar, Alert } from '@mui/material';


const PatientSkinSensoryAid = () => {
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


  // Load saved or fetched data, and remember initial state
  useEffect(() => {
    const saved = localStorage.getItem(`patient-skinsensoryaid-${id}`);
    if (saved) {
      const parsed = JSON.parse(saved);
      setAnswers(parsed);
      setInitialAnswers(parsed);
    } else {
      fetchPatientData();
    }
  }, [id]);

  const fetchPatientData = async () => {
    try {
      const { data } = await axios.get(
        `${APIHOST}/api/patients/nurse/patient/${id}/skinandsensoryaid`
      );
      setAnswers(data);
      setInitialAnswers(data);
    } catch (err) {
      console.error('Error fetching patient:', err);
    }
  };

  const handleAnswerChange = (question, answer) => {
    setAnswers(prev => ({ ...prev, [question]: answer }));
  };

  // Only saves when something changed
  const handleSave = () => {
    try {
      localStorage.setItem(
        `patient-skinsensoryaid-${id}`,
        JSON.stringify(answers)
      );
      setInitialAnswers(answers);
      setSnackbar({
        open: true,
        message: 'Patient record saved successfully!',
        severity: 'success'
      });
    } catch (err) {
      console.error('Error saving data:', err);
      setSnackbar({
        open: true,
        message: 'Error: Failed to save patient data.',
        severity: 'error'
      });
    }
  };

  // Compare JSON to detect changes
  const isDirty = () =>
    JSON.stringify(answers) !== JSON.stringify(initialAnswers);

  const questions = [
    { id: 'glasses', text: 'Glasses' },
    { id: 'hearing', text: 'Hearing' },
  ];

  return (
    <div className="container mt-4 d-flex assessment-page">
      <AssessmentsCard />
      <div className="ms-4 flex-fill">
        <div className="d-flex justify-content-between align-items-center mb-4 assessment-header">
          <text>Sensory Aids / Prothesis / Skin Integrity</text>
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

        {/* Skin Integrity */}
        <Card className="mt-4 gradient-background">
          <Card.Body>
            <Form>
              <div className="mb-2 d-flex justify-content-end">
                <strong className="me-4">Yes</strong>
                <strong>No</strong>
              </div>
              <Form.Group className="mb-3 d-flex align-items-center">
                <Form.Label className="me-3">Skin Integrity:</Form.Label>
                <div className="ms-auto d-flex align-items-center">
                  <Form.Check
                    inline
                    name="skinIntegrity"
                    type="radio"
                    id="skinIntegrity-yes"
                    checked={answers.skinIntegrity === 'yes'}
                    onChange={() =>
                      handleAnswerChange('skinIntegrity', 'yes')
                    }
                  />
                  <Form.Check
                    inline
                    name="skinIntegrity"
                    type="radio"
                    id="skinIntegrity-no"
                    checked={answers.skinIntegrity === 'no'}
                    onChange={() =>
                      handleAnswerChange('skinIntegrity', 'no')
                    }
                  />
                </div>
              </Form.Group>

              {answers.skinIntegrity === 'yes' && (
                <Form.Group className="mb-3">
                  <Form.Label>Frequency</Form.Label>
                  <div className="d-flex align-items-center">
                    {['Q2h', 'Q4h', 'QShift'].map(freq => (
                      <Form.Check
                        inline
                        key={freq}
                        name="skinIntegrityFrequency"
                        type="radio"
                        label={freq}
                        checked={answers.skinIntegrityFrequency === freq}
                        onChange={() =>
                          handleAnswerChange(
                            'skinIntegrityFrequency',
                            freq
                          )
                        }
                      />
                    ))}
                  </div>
                </Form.Group>
              )}
            </Form>
          </Card.Body>
        </Card>

        {/* Glasses & Hearing */}
        <Card className="mt-4 gradient-background">
          <Card.Body>
            <Form>
              <div className="mb-2 d-flex justify-content-end">
                <strong className="me-4">Yes</strong>
                <strong>No</strong>
              </div>
              {questions.map((q, i) => (
                <Form.Group
                  key={i}
                  className="mb-3 d-flex align-items-center"
                >
                  <Form.Label className="me-3">{q.text}:</Form.Label>
                  <div className="ms-auto d-flex align-items-center">
                    <Form.Check
                      inline
                      name={q.id}
                      type="radio"
                      id={`${q.id}-yes`}
                      checked={answers[q.id] === 'yes'}
                      onChange={() => handleAnswerChange(q.id, 'yes')}
                    />
                    <Form.Check
                      inline
                      name={q.id}
                      type="radio"
                      id={`${q.id}-no`}
                      checked={answers[q.id] === 'no'}
                      onChange={() => handleAnswerChange(q.id, 'no')}
                    />
                  </div>
                </Form.Group>
              ))}
            </Form>
          </Card.Body>
        </Card>

        {/* Pressure Ulcer Risk */}
        <Card className="mt-4 gradient-background">
          <Card.Body>
            <Form>
              <Form.Group className="mb-3">
                <Form.Label>Pressure Ulcer Risk:</Form.Label>
                <div className="d-flex align-items-center">
                  {['Low', 'Medium', 'High', 'Very High'].map(label => (
                    <Form.Check
                      inline
                      key={label}
                      name="pressureUlcerRisk"
                      type="radio"
                      label={label}
                      checked={answers.pressureUlcerRisk === label}
                      onChange={() =>
                        handleAnswerChange('pressureUlcerRisk', label)
                      }
                    />
                  ))}
                </div>
              </Form.Group>
            </Form>
          </Card.Body>
        </Card>

        {/* Skin Integrity - Turning Schedule */}
        <Card className="mt-4 gradient-background">
          <Card.Body>
            <Form>
              <div className="mb-2 d-flex justify-content-end">
                <strong className="me-4">Yes</strong>
                <strong>No</strong>
              </div>
              <Form.Group className="mb-3 d-flex align-items-center">
                <Form.Label className="me-3">
                  Skin Integrity - Turning Schedule:
                </Form.Label>
                <div className="ms-auto d-flex align-items-center">
                  <Form.Check
                    inline
                    name="skinIntegrityTurningSchedule"
                    type="radio"
                    id="turningSchedule-yes"
                    checked={answers.skinIntegrityTurningSchedule === 'yes'}
                    onChange={() =>
                      handleAnswerChange(
                        'skinIntegrityTurningSchedule',
                        'yes'
                      )
                    }
                  />
                  <Form.Check
                    inline
                    name="skinIntegrityTurningSchedule"
                    type="radio"
                    id="turningSchedule-no"
                    checked={answers.skinIntegrityTurningSchedule === 'no'}
                    onChange={() =>
                      handleAnswerChange(
                        'skinIntegrityTurningSchedule',
                        'no'
                      )
                    }
                  />
                </div>
              </Form.Group>

              {answers.skinIntegrityTurningSchedule === 'yes' && (
                <Form.Group className="mb-3">
                  <Form.Label>Frequency:</Form.Label>
                  <div className="d-flex align-items-center">
                    {['Q2h', 'Q4h', 'QShift'].map(freq => (
                      <Form.Check
                        inline
                        key={freq}
                        name="turningScheduleFrequency"
                        type="radio"
                        label={freq}
                        checked={answers.turningScheduleFrequency === freq}
                        onChange={() =>
                          handleAnswerChange(
                            'turningScheduleFrequency',
                            freq
                          )
                        }
                      />
                    ))}
                  </div>
                </Form.Group>
              )}
            </Form>
          </Card.Body>
        </Card>

        {/* Skin Integrity - Dressings */}
        <Card className="mt-4 gradient-background">
          <Card.Body>
            <Form>
              <Form.Group className="mb-3">
                <Form.Label>Skin Integrity - Dressings:</Form.Label>
                <Form.Control
                  type="text"
                  value={answers.skinIntegrityDressings || ''}
                  onChange={e =>
                    handleAnswerChange('skinIntegrityDressings', e.target.value)
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

export default PatientSkinSensoryAid;
