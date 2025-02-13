import React from 'react'
import moment from "moment-jalaali";

const notification_card = (notificationItem) => {

    let notification = notificationItem.notificationItem;

    return (
        <div className={`block max-w-sm p-6 bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700 ${!notification.isRead && 'dark:hover:bg-gray-700 hover:bg-gray-100 cursor-pointer'}`}>
            <span className="font-normal text-gray-700 dark:text-gray-400 flex flex-row-reverse justify-between" dir='ltr'>
                {moment(notification.createdAt).format("jYYYY-jMM-jDD | HH:mm:ss")} {/* convert to persian date */}
                {notification.isRead && (
                    <i class="fa-solid fa-check-double me-2" aria-hidden="true"></i>
                )}
            </span>
            <h5 className="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">{notification.messageSubject}</h5>
            <p className="font-normal text-gray-700 dark:text-gray-400">{notification.messageContent}</p>
        </div>
    )
}

export default notification_card