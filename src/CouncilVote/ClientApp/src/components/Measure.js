import React, { useState, useEffect } from 'react';
import {Badge} from 'reactstrap';
import CastVote from './CastVote';
import { getMeasure } from '../repo/loadData';

const initialState = {
    subject: "",
    description: "",
    votes: [],
    status: {
        case: "Active"
    }
}

function Measure(props) {
    const [measure, setMeasure] = useState(initialState);
    const { id } = props.match.params;
    
    const fetchMeasure = async () => {
        const measure = await getMeasure(id);
        setMeasure(measure);
    }
    
    useEffect(() => {
      fetchMeasure();
    }, [])
    
    return (
        <div>
            <h1 className="display-4">{measure.subject}</h1>
            <p>{measure.description}</p>
            <br/>
            <div>
                <h3>
                    Measure Status
                    {(() => {
                        switch (measure.status.case) {
                            case 'Passed': return <Badge color="success">{measure.status.case}</Badge>
                            case 'Failed': return <Badge color="danger">{measure.status.case}</Badge>
                            default: return <Badge color="primary">{measure.status.case}</Badge>
                        }
                    })()}
                </h3>
                <h4>Number of votes: <Badge color="secondary">{measure.votes.length}</Badge></h4>
            </div>
            
            <br/>
            <br/>
            <CastVote measureId={id} updateMeasure={m => setMeasure(m)}/>
        </div>
    )
}

export default Measure;