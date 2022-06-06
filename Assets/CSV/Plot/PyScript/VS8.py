# Common imports
import pandas as pd
import numpy as np
from sklearn import preprocessing
# import seaborn as sns
# sns.set(color_codes=True)
import matplotlib.pyplot as plt


path = "C:\\Users\\Amirashkan\\Desktop\\Unity\\Shared Project\\Backup\\02142022\\DT_WindFarm_MasterThesis\\Assets\\CSV\\"
# merged_data = pd.read_csv("C:\\Users\\Amirashkan\\Desktop\\Unity\\Shared Project\\Backup\\02142022\\DT_WindFarm_MasterThesis\\Assets\\CSV\\Bearing_Vibration.csv")
# merged_data = pd.read_csv("C:\\Users\\Amirashkan\\Desktop\\Unity\\Shared Project\\Backup\\02142022\\DT_WindFarm_MasterThesis\\Assets\\CSV\\BearingTemp_2.csv")
# realdata = pd.read_csv("C:\\Users\\Amirashkan\\Desktop\\Unity\\Shared Project\\Backup\\02142022\\DT_WindFarm_MasterThesis\\Assets\\CSV\\Saved_data.csv")
merged_data = pd.read_csv(path + "Saved_data - Backup.csv")

# merged_data.index = pd.to_datetime(merged_data.index, format='%Y.%m.%d.%H.%M.%S',errors='ignore')
# merged_data.head(800)
print(f'Total Data Points {merged_data.shape[0] + 1}')
# Visualising Data
# ax = merged_data.plot(figsize = (16,6), title="Temperature Data" , legend = True)
# ax.set(xlabel="Year-Month-Date", ylabel="Temperature (C)")


Temp_data = merged_data.copy() 
# Temp_data[Temp_data.columns[0]] = pd.to_datetime(Temp_data[Temp_data.columns[0]], format='%Y-%m-%d')
Temp_data[Temp_data.columns[0]] = pd.to_datetime(Temp_data[Temp_data.columns[0]])
Temp_data = Temp_data.set_index(Temp_data.columns[0])
Temp_data = Temp_data.sort_index()
# all_Turbines = Temp_data.astype('float')


value = 'Value8'

length = len(Temp_data)
minRange = int(length * 5/100)
maxRange = int(length * 80/100)


#Chossing data for training
healthy_bearing1 = Temp_data[Temp_data.index[0]:Temp_data.index[maxRange]][value]

# Creating training dataframe
prophet_healthy_train = pd.DataFrame()
prophet_healthy_train['ds'] = healthy_bearing1.index
prophet_healthy_train['y'] = healthy_bearing1.values


# prophet_healthy_train2.head()
from fbprophet import Prophet
m = Prophet(yearly_seasonality=False, weekly_seasonality=False,daily_seasonality=False, mcmc_samples=0, growth='linear', interval_width= 1)
# Using the training data from "healthy part"
m.add_seasonality(name='Secondly', period= 1,fourier_order= 1 , mode ='multiplicative') 
m.fit(prophet_healthy_train)


forecast = m.predict(prophet_healthy_train)
forecast['fact'] = prophet_healthy_train['y'].reset_index(drop = True)
# print(forecast.tail())

forecast_data_orig = forecast # make sure we save the original forecast data
# forecast_data_orig['yhat'] = (forecast_data_orig['yhat'])
# forecast_data_orig['yhat_lower_1'] = (forecast_data_orig['yhat_lower'])
# forecast_data_orig['yhat_upper_1'] = (forecast_data_orig['yhat_upper'])
# m.plot(forecast_data_orig)
# forecast.to_csv(path + 'Predicted2.csv',index=False)
# forecast_data_orig[['yhat_lower_1','yhat_upper_1']].to_csv(path + 'Predicted.csv',columns =["2","3"] ,index=False)
indx1 = 14
indx2 = 15
x = pd.read_csv(path + "Predicted.csv")
df = pd.DataFrame(x);
df.drop(df.columns[[indx1,indx2]], inplace=True, axis=1)
df.insert(loc= indx1, column='yhat_lower'+ str(indx1), value= forecast_data_orig['yhat_lower'])
df.insert(loc= indx2, column='yhat_upper'+ str(indx1), value= forecast_data_orig['yhat_upper'])
df.to_csv(path + 'Predicted.csv',index=False)

# print('Displaying Prophet plot')
# fig1 = m.plot(forecast)
# fig1 = healthy_bearing1.plot(figsize = (20,6), title="Fit of Training Data")
# fig1.set(xlabel="Month (MM)-Date(DD) Time", ylabel="Temperature(C)")
prophet_faultydata = Temp_data[Temp_data.index[maxRange]:Temp_data.index[-1]][value]
# prophet_faultydata.head()
prophet_faulty_test = pd.DataFrame()
prophet_faulty_test['ds'] = prophet_faultydata.index
prophet_faulty_test['y'] = prophet_faultydata.values
ftr = int((length * 70) / 100)
future_data = m.make_future_dataframe(periods = ftr, freq = 's')
forecast = m.predict(future_data)
# m.plot_components(forecast)
forecast['fact'] = prophet_faulty_test['y'].reset_index(drop = True)
# print('Displaying Prophet plot')
fig = m.plot(forecast,figsize=(16, 6))
plt.scatter(prophet_faultydata.index, prophet_faultydata.values, c = 'r', s= 10.0, alpha=0.7)
plt.legend(["Actual/Trained Data","Expected/Predicted",'Acceptable variance','Actual/Test Data'] , fontsize=15)
plt.xlabel("Date Time",size=15)
plt.ylabel("Temperature(C)",size=15)
plt.title("Turbine 8",size=15)

# print(fig1.get_xticks())
# print(fig1.get_yticks())
# plt.plot()
plt.show()

# plt.ion()

