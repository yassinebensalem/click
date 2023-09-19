import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { makeStyles, withStyles } from '@material-ui/core/styles';
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

const useStyles = makeStyles({
    table: {
        minWidth: 650
    },
    tableRow: {
        height: 30
    },
    tableCell: {
        padding: "0px 16px"
    }
});

const StyledTableRow = withStyles((theme) => ({
    root: {
        height: 10
    }
}))(TableRow);

const AuthorRowDashboard = (props) => {
    const { row } = props;
    const classes = useStyles();

    return (
        <React.Fragment>

            <StyledTableRow
            >
                <TableCell component="th" scope="row">
                    {row.authorName}
                </TableCell>

                <TableCell component="th" scope="row">
                    {row.booksCount}
                </TableCell>
            </StyledTableRow>



        </React.Fragment>
    );
}
AuthorRowDashboard.propTypes = {

};

export default AuthorRowDashboard;