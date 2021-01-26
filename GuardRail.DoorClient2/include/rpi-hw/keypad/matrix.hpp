/* 
    Title --- keypad/matrix.hpp

    Copyright (C) 2013 Giacomo Trudu - wicker25[at]gmail[dot]com

    This file is part of Rpi-hw.

    Rpi-hw is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation version 3 of the License.

    Rpi-hw is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Rpi-hw. If not, see <http://www.gnu.org/licenses/>.
*/


#ifndef _RPI_HW_KEYPAD_MATRIX_HPP_
#define _RPI_HW_KEYPAD_MATRIX_HPP_

#include "../types.hpp"
#include "../exception.hpp"
#include "../utils.hpp"
#include "../time.hpp"
#include "../consts.hpp"
#include "../math.hpp"

#include "../iface/base.hpp"
#include "../iface/output.hpp"
#include "../iface/input.hpp"

#include "base.hpp"

namespace rpihw { // Begin main namespace

namespace keypad { // Begin keypads namespace

/*!
	@class matrix
	@brief Matrix keypad controller.

	@example keypad/12keys0.cpp
	@example keypad/12keys1.cpp
	@example keypad/16keys0.cpp
	@example keypad/16keys1.cpp
	@example keypad/16keys2.cpp
*/
class matrix : public keypad::base {

public:

	/*!
		@brief Constructor method.
		@param[in] cols Sequence of `uint8_t` containing the column GPIOs.
		@param[in] rows Sequence of `uint8_t` containing the rows GPIOs.
	*/
	matrix( std::initializer_list< uint8_t > cols, std::initializer_list< uint8_t > rows );

	/*!
		@brief Constructor method.
		@param[in] cols Sequence of `uint8_t` containing the column GPIOs.
		@param[in] rows Sequence of `uint8_t` containing the rows GPIOs.
		@param[in] keymap The keymap vector.
	*/
	matrix( std::initializer_list< uint8_t > cols, std::initializer_list< uint8_t > rows, const std::vector< uint8_t > &keymap );

	//! Destructor method.
	virtual ~matrix();

protected:

	//! Columns output interface.
	iface::output *m_output;

	//! Updates the state of buttons.
	virtual void update();
};

} // End of keypads namespace

} // End of main namespace


// Include inline methods 
#include "matrix-inl.hpp"

#endif /* _RPI_HW_KEYPAD_MATRIX_HPP_ */
