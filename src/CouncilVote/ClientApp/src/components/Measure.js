import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col } from 'reactstrap';
import { getMeasure } from '../repo/loadData';

function Measure(props) {
    const [measure, setMeasure] = useState({ subject: "" });
    const { id } = props.match.params;
    
    useEffect(() => {
      const loadData = async () => {
          const measure = await getMeasure(id);
          setMeasure(measure);
      }
      loadData();
    }, [])
    
    return (
        <div>
            <h1 className="display-4">{measure.subject}</h1>
            <p>{measure.description}</p>
        </div>
    )
}

export default Measure;