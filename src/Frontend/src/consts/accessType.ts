export enum CourseRoleType {
	courseAdmin = 'courseAdmin',
	instructor = 'instructor',
	student = 'student',
	tester = 'tester',
}

export enum SystemAccessType {
	viewAllProfiles = 'viewAllProfiles',
	viewAllGroupMembers = 'viewAllGroupMembers',
}

export enum CourseAccessType {
	editPinAndRemoveComments = 'editPinAndRemoveComments',
	viewAllStudentsSubmissions = 'viewAllStudentsSubmissions',
	addAndRemoveInstructors = 'addAndRemoveInstructors',
	apiViewCodeReviewStatistics = 'apiViewCodeReviewStatistics',
}

export const AccessType = { ...CourseAccessType, ...SystemAccessType };
export type AccessType = typeof AccessType;
