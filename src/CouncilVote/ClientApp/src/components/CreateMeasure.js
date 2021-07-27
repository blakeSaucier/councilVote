import React, { useState } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col } from 'reactstrap';
import { createMeasure } from '../repo/loadData';

const initialState = {
  subject: "",
  description: "",
  minimumVotesSelected: false,
  minimumVotes: 0,
  minimumYesSelected: false,
  minimumYesPercent: 0.0,
  requiredVotersSelected: false,
  requiredVoters: []
}

function CreateMeasure(props) {
  const [formState, setFormState] = useState(initialState)
  
  const handleSubmit = async e => {
    e.preventDefault();
    const { id } = await createMeasure(formState);
    props.history.push(`/measure/${id}`);
  }
  
  const formDataChanged = e => {
    const result = {
      ...formState,
      [e.target.name]: e.target.value
    };
    setFormState(result);
  }
  
  const checkBoxChanged = e => {
    const result = {
      ...formState,
      [e.target.name]: e.target.checked
    }
    setFormState(result);
  }
  
  const numberChanged = e => {
      const num = parseInt(e.target.value);
      const result = {
          ...formState,
          [e.target.name]: num
      }
      setFormState(result);
  }
    
  return(
    <Form onSubmit={handleSubmit}>
      <h2>Create New Measure</h2>
      <FormGroup>
        <Label for="subject">Subject</Label>
        <Input type="text" 
               name="subject" 
               id="subject" 
               placeholder="Measure Subject" 
               value={formState.subject}
               onChange={formDataChanged}
        />
      </FormGroup>
      <FormGroup>
        <Label for="description">Description</Label>
        <Input type="textarea"
               name="description" 
               id="description" 
               value={formState.description} 
               onChange={formDataChanged} />
      </FormGroup>
      <Row form>
        <Col md={6}>
          <FormGroup check>
              <Label check>
                <Input id="minimumVotesSelected" 
                       name="minimumVotesSelected" 
                       type="checkbox" 
                       value={formState.minimumVotesSelected}
                       onChange={checkBoxChanged}/> This measure requires a minimum number of votes
              </Label>
          </FormGroup>
        </Col>
        <Col md={6}>
            <FormGroup>
                <Label for="minimumVotes">Mininum number of votes</Label>
                <Input name="minimumVotes" 
                       id="minimumVotes" 
                       type="number" 
                       value={formState.minimumVotes}
                       onChange={numberChanged}/>
            </FormGroup>
        </Col>
      </Row>
        <Row form>
            <Col md={6}>
                <FormGroup check>
                    <Label check>
                        <Input id="minimumYesSelected" 
                               name="minimumYesSelected" 
                               type="checkbox" 
                               value={formState.minimYesSelected}
                               onChange={checkBoxChanged}/> 
                        This measure requires a minimum percent of YES votes
                    </Label>
                </FormGroup>
            </Col>
            <Col md={6}>
                <FormGroup>
                    <Label for="minimumYesPercent">Minimum Percent YES</Label>
                    <Input name="minimumYesPercent" 
                           id="minimumYesPercent" 
                           type="number" 
                           placeholder="50" 
                           value={formState.minimumYesPercent}
                           onChange={numberChanged}/>
                </FormGroup>
            </Col>
        </Row>
      <Button type="submit">Create</Button>
    </Form>
  );
}

export default CreateMeasure;