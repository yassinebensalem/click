import React from 'react';
import { Card } from '@material-ui/core'
const NoteCard = ({ children, height, flex }) => {
    return (
        <Card
            style={{
                flex,
                padding: '10px',
                height,
                "boxShadow": "2px 4px 10px 1px rgba(201, 201, 201, 0.47)",
                borderRadius: "10px",
            }}
        >
            <div className="flex justify-between">
                {children}
            </div>
        </Card>
    )
}

export default NoteCard;