import React from 'react'
import { CircularProgress } from '@material-ui/core';
const Loading = () => {

        return (
            <div style={{ display: 'flex', justifyContent: 'center', marginTop: '20%' }}>
                    <CircularProgress color="#88CF00" />
                </div>
        )

}

export default Loading
