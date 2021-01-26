/* 
    Title --- keypad/matrix.cpp

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


#ifndef _RPI_HW_KEYPAD_MATRIX_CPP_
#define _RPI_HW_KEYPAD_MATRIX_CPP_
#include "../../include/rpi-hw/keypad/matrix.hpp"

namespace rpihw { // Begin main namespace

namespace keypad { // Begin keypads namespace

matrix::matrix( std::initializer_list< uint8_t > cols, std::initializer_list< uint8_t > rows )

	: keypad::keypad_base	( cols.size() * rows.size(), rows )
	, m_output		( new iface::output( cols ) ) {

}

matrix::matrix( std::initializer_list< uint8_t > cols, std::initializer_list< uint8_t > rows, const std::vector< uint8_t > &keymap )

	: keypad::keypad_base( cols.size() * rows.size(), rows, keymap )
	, m_output		( new iface::output( cols ) ) {

}

matrix::~matrix() {

	// Destroy the interfaces
	delete m_input;
}

void matrix::update() {

	// Get the size of the keypad
	uint8_t	cols = m_output->size(), rows = m_input->size();

	// Current button index
	size_t index;

	// Button state
	bool state;

	// Iterators
	uint8_t i, j;

	// Updating loop
	for ( ;; ) {

		// Activate all columns
		m_output->write( 0 );

		// Check if some buttons have been pressed
		if ( ~m_input->read() == 0 )
			continue;

		// Deactivate all columns
		m_output->write( 0xFFFF );


		// Update state of buttons
		for ( j = 0; j < cols; ++j ) {

			// Activate the j-th column
			m_output->write( j, LOW );

			for ( i = 0; i < rows; ++i ) {

				// Look for connection with i-th row
				state = !m_input->read(i);

				// Update the button registers
				index = static_cast<size_t>(j) + static_cast<size_t>(i) * static_cast<size_t>(cols);

				m_mutex->lock();

				m_pressed[ index ]	= !m_keystate[ index ] && state;
				m_released[ index ]	= m_keystate[ index ] && !state;
				m_keystate[ index ]	= state;

				m_mutex->unlock();
			}

			// Deactivate the j-th column
			m_output->write( j, HIGH );
		}

		// Call the event listener
		if ( m_event_listener ) {

			m_mutex->lock();
			m_event_listener( *this );
			m_mutex->unlock();
		}

		// Wait some time
		time::usleep( math::floor( 1000000.0 / m_frequency ) );
	}
}

} // End of keypads namespace

} // End of main namespace

#endif /* _RPI_HW_KEYPAD_MATRIX_CPP_ */
