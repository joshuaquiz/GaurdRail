/* 
    Title --- iface/encoder-in.hpp

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


#ifndef _RPI_HW_IFACE_ENCODER_IN_HPP_
#define _RPI_HW_IFACE_ENCODER_IN_HPP_

#include <initializer_list>
#include <vector>

#include "../types.hpp"
#include "../exception.hpp"
#include "../math.hpp"
#include "../utils.hpp"

#include "iface_base.hpp"
#include "input.hpp"

namespace rpihw { // Begin main namespace

namespace iface { // Begin interfaces namespace

/*!
	@class encoderIn
	@brief Encoder input interface.
*/
class encoderIn : public iface::input {

public:

	/*!
		@brief Constructor method.
		@param[in] pins Sequence of `uint8_t` containing the GPIO pins.
	*/
	encoderIn( std::initializer_list< uint8_t > pins );

	/*!
		@brief Constructor method.
		@param[in] pins Vector containing the GPIO pins.
	*/
	encoderIn( const std::vector< uint8_t > &pins );

	//! Destructor method.
	virtual ~encoderIn();

	/*!
		@brief Reads from the interface.
		@return The data read from the interface.
	*/
	virtual size_t read() const;
};

} // End of interfaces namespace

} // End of main namespace


// Include inline methods 
#include "encoder-in-inl.hpp"

#endif /* _RPI_HW_IFACE_ENCODER_IN_HPP_ */
