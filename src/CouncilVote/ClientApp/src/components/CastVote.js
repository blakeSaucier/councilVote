import React, { useState } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col } from 'reactstrap';
import { submitVote } from '../repo/loadData';

const initialState = {
  inFavor: false,
  name: ""
}

function CastVote(props) {
    const { measureId } = props;
    const [vote, setVote] = useState({ measureId, ...initialState })
    
    
    const handleSubmit = async e => {
        e.preventDefault();
        const res = await submitVote(vote);
        props.updateMeasure(res);
        setVote({
            ...vote,
            name: ""
        });
    }

    const onNameChange = e => {
        const res = {
            ...vote,
            name: e.target.value
        }
        setVote(res);
    }
    
    const onVoteChange = e => {
        const res = {
            ...vote,
            inFavor: (e.target.value === 'true')
        }
        setVote(res);
    }
    
    return(
        <Row>
            <Col sm={3}>
                <Form onSubmit={handleSubmit}>
                    <h2>Cast Vote</h2>
                    <FormGroup>
                        <Label for="name">Name</Label>
                        <Input name="name"
                               id="name"
                               type="text"
                               value={vote.name}
                               onChange={onNameChange} />
                    </FormGroup>
                    <FormGroup check>
                        <Label>
                            <Input name="inFavor"
                                   id="inFavor"
                                   type="radio"
                                   value={true}
                                   onChange={onVoteChange}
                            />
                            Vote In Favor
                        </Label>
                    </FormGroup>
                    <FormGroup check>
                        <Label>
                            <Input name="inFavor"
                                   id="inFavor"
                                   type="radio"
                                   value={false}
                                   onChange={onVoteChange}/>
                            Vote Against
                        </Label>
                    </FormGroup>
                    <Button type="submit">Submit Vote</Button>
                </Form>
            </Col>
        </Row>
    );
}

export default CastVote;