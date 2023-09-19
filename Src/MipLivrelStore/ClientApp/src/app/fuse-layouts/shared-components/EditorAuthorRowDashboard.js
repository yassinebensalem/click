import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { makeStyles } from '@material-ui/core/styles';
import Box from '@material-ui/core/Box';
import Collapse from '@material-ui/core/Collapse';
import IconButton from '@material-ui/core/IconButton';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Typography from '@material-ui/core/Typography';
import Paper from '@material-ui/core/Paper';

import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import Container from '@material-ui/core/Container';
import Divider from '@material-ui/core/Divider';

const useRowStyles = makeStyles({
    root: {

    },
});

const EditorAuthorRowDashboard = (props) => {
    const { row } = props;
    const classes = useRowStyles();



    return (<React.Fragment>

        <TableRow className={classes.root} >
            <TableCell component="th" scope="row"
            >
                {row.name}
            </TableCell>

            <TableCell component="th" scope="row">
                {row.total}
            </TableCell>
        </TableRow>



    </React.Fragment>)

}
EditorAuthorRowDashboard.propTypes = {

};

export default EditorAuthorRowDashboard;