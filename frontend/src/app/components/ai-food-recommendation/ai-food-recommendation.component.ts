import { NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment';

interface RecommendationResponse {
	recommendations: string;
}

@Component({
	selector: 'app-ai-food-recommendation',
	templateUrl: './ai-food-recommendation.component.html',
	standalone: true,
	imports: [FormsModule, NgIf]
})
export class AiFoodRecommendationComponent {
	userPrompt: string = '';
	aiResponse: string = '';
	isLoading: boolean = false;
	error: string | null = null;

	constructor(private http: HttpClient) {}

	async getFoodRecommendation() {
		if (!this.userPrompt.trim()) {
			alert('Please enter your health condition or dietary requirement.');
			return;
		}

		this.isLoading = true;
		this.aiResponse = '';
		this.error = null;

		try {
			const response = await this.http
				.post<RecommendationResponse>(`${environment.apiUrl}api/AiFoodRecommendation/recommend`, JSON.stringify(this.userPrompt), {
					headers: {
						'Content-Type': 'application/json'
					}
				})
				.toPromise();

			if (response?.recommendations) {
				// Format the recommendations for better display
				this.aiResponse = this.formatRecommendations(response.recommendations);
			} else {
				this.error = 'No recommendations received. Please try again.';
				this.aiResponse = 'Failed to get recommendations. Please try again.';
			}
		} catch (error) {
			console.error('Error fetching AI response:', error);
			this.error = 'Failed to fetch recommendations. Please try again.';
			this.aiResponse = 'Failed to get recommendations. Please try again.';
		} finally {
			this.isLoading = false;
		}
	}

	private formatRecommendations(recommendations: string): string {
		return recommendations
			.replace(/\*\*/g, '') // Remove markdown bold syntax
			.split('\n')
			.filter(line => line.trim()) // Remove empty lines
			.map(line => line.replace(/^-\s*/, '')) // Remove leading dash
			.join('\n\n'); // Add extra spacing between recommendations
	}
}
